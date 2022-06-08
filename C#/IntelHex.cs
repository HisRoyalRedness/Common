using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HisRoyalRedness.com.IntelHex;

namespace HisRoyalRedness.com
{
    public class IntelHexReader : Stream
    {
        // https://en.wikipedia.org/wiki/Intel_HEX
        public IntelHexReader(Stream stream, 
            Record record = null) // Used by unit testing to provide a Record instance
        {
            _record = record ?? new Record();
            _states = new()
            {
                { States.StartOfRecord, new State_StartOfRecord(this) },
                { States.EndOfStream, new State_EndOfStream(this) },
                { States.StartCode, new State_StartCode(this, _record) },
                { States.EndOfRecord, new State_EndOfRecord(this) },
                { States.EndOfFile, new State_EndOfFile(this) },
            };

            // Queue up the first token
            _tokens = IntelHex.Tokeniser.GetTokens(stream).GetEnumerator();
            _currentState = _states[States.StartOfRecord];
        }

        public IntelHexReader(string stream, Record record = null) : this(new MemoryStream(Encoding.UTF8.GetBytes(stream)), record)
        { }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool EndOfStream => _currentState.StateId == States.EndOfStream;

        public UInt32 CurrentAddress { get; internal set; } = 0;

        public byte Read()
        {
            byte[] buffer = new byte[1];
            var bytesRead = Read(buffer, 0, buffer.Length);
            if (bytesRead != 1)
                throw new ApplicationException("Could not read from the stream");
            return buffer[0];
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = 0;

            // Do we have data left over from the last read?
            if (_record.Data.Count > 0)
            {
                var bytesCopied = Math.Min((count - bytesRead), _record.Data.Count);
                for (int i = 0; i < bytesCopied; ++i)
                    buffer[offset++] = _record.Data.Dequeue();
                bytesRead += bytesCopied;
                CurrentAddress = CurrentAddress + (uint)bytesRead;
            }

            // Keep running around the loop and through the states until we get
            // the number of bytes we were looking for.
            // Keep reading the full line, even if we have enough data.
            // The extra will be returned on the next read
            while ((bytesRead < count || !_record.LineCompleted) &&
                !EndOfStream &&
                _tokens.MoveNext())
            {
                var token = _tokens.Current;

                // An invalid token is invalid regardless of state
                if (token.Type == TokenType.Invalid)
                    throw new FileFormatException($"Invalid token encountered (line {token.LineNumber}, col {token.ColumnNumber})", token);

                // Process the current token and potentially transition to the new state
                var newStateId = _currentState.ProcessToken(token);
                if (newStateId != States.SameState)
                {
                    if (_states.ContainsKey(newStateId))
                    {
                        _currentState.Exit();
                        _currentState = _states[newStateId];
                        _currentState.Enter();
                    }
                    else
                        throw new ApplicationException($"Invalid state {newStateId}. Has it been added to the state dictionary?");
                }

                // Do we have some of the data? Copy it over now
                if (_record.LineCompleted && _record.Data.Count > 0)
                {
                    var bytesCopied = Math.Min((count - bytesRead), _record.Data.Count);
                    for (int i = 0; i < bytesCopied; ++i)
                        buffer[offset++] = _record.Data.Dequeue();
                    bytesRead += bytesCopied;
                    CurrentAddress = CurrentAddress + (uint)bytesRead;
                }
            }

            return bytesRead;
        }

        // Stuff not supported on a reader stream
        public override void Flush() => throw new NotSupportedException("Not supported by a reader stream");
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException("Not supported by a reader stream");
        public override void SetLength(long value) => throw new NotSupportedException("Not supported by a reader stream");
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException("Not supported by a reader stream");

        #region States
        internal enum RecordTypes
        {
            Data = 0,
            EndOfFile = 1,
            ExtendedSegmentAddress = 2,
            StartSegmentAddress = 3,
            ExtendedLinearAddress = 4,
            StartLinearAddress = 5
        }

        internal enum States
        {
            SameState,
            StartOfFile,
            StartOfRecord,      // The beginning of the record. We've not identified the start code yet
            StartCode,          // We've encountered the start code and we're reading data
            EndOfRecord,        // We've read all the data we expect. Anticipate another line, or end of stream
            EndOfFile,
            EndOfStream
        }

        internal abstract class StateBase
        {
            protected StateBase(IntelHexReader reader, States stateId)
            {
                StateId = stateId;
                Reader = reader;
            }

            public void Enter()
            {
                InternalEnter();
            }

            public void Exit()
            {
                InternalExit();
            }

            protected virtual void InternalEnter() { }
            protected virtual void InternalExit() { }

            public abstract States ProcessToken(Token token);

            public States StateId { get; private set; }

            protected IntelHexReader Reader { get; private set; }
        }

        internal class State_StartOfRecord : StateBase
        {
            public State_StartOfRecord(IntelHexReader reader) : base(reader, States.StartOfRecord)
            { }

            public override States ProcessToken(Token token)
            {
                return token.Type switch
                {
                    TokenType.EndOfStream => States.EndOfStream,// End of stream
                    TokenType.EndOfLine => States.SameState,// End of line, do nothing
                    TokenType.StartCode => States.StartCode,// Start code
                    _ => States.SameState,// Ignore everything else
                };
            }
        }

        internal class State_EndOfStream : StateBase
        {
            public State_EndOfStream(IntelHexReader reader) : base(reader, States.EndOfStream)
            { }

            public override States ProcessToken(Token token)
            {
                if (token.Type == TokenType.EndOfStream)
                    return States.SameState;

                throw new FileFormatException($"Unexpected {token.Type} token after End of Stream", token);
            }
        }

        internal class State_StartCode : StateBase
        {
            public State_StartCode(IntelHexReader reader, Record record) : base(reader, States.StartCode)
            {
                _record = record;
            }

            public override States ProcessToken(Token token)
            {
                switch(token.Type)
                {
                    case TokenType.Hex:
                        switch(_state)
                        {
                            case LineState.ByteCount:
                                _record.ByteCount = token.Value;
                                _record.AddToCRC(token.Value);
                                _state = LineState.Address1;
                                return States.SameState;

                            case LineState.Address1:
                                Reader.CurrentAddress += ((uint)token.Value << 8);
                                _record.AddToCRC(token.Value);
                                _state = LineState.Address2;
                                return States.SameState;

                            case LineState.Address2:
                                Reader.CurrentAddress += token.Value;
                                _record.AddToCRC(token.Value);
                                _state = LineState.RecordType;
                                return States.SameState;

                            case LineState.RecordType:
                                _record.RecordType = token.Value;

                                switch ((RecordTypes)_record.RecordType)
                                {
                                    case RecordTypes.Data:
                                        if (_record.ByteCount == 0)
                                            throw new FileFormatException("No data for a Data record", token);
                                        break;

                                    case RecordTypes.EndOfFile:
                                        if (_record.ByteCount != 0)
                                            throw new FileFormatException("Unexpected data for an End Of File record", token);
                                        break;

                                    case RecordTypes.ExtendedSegmentAddress:
                                    case RecordTypes.ExtendedLinearAddress:
                                        if (_record.ByteCount != 2)
                                            throw new FileFormatException("Incorrect byte count for an Extended Address record", token);
                                        break;

                                    case RecordTypes.StartSegmentAddress:
                                    case RecordTypes.StartLinearAddress:
                                        if (_record.ByteCount != 4)
                                            throw new FileFormatException("Incorrect byte count for an Start Address record", token);
                                        break;

                                    default:
                                        throw new FileFormatException($"Unknown record type {token.Value}", token);
                                }

                                _record.AddToCRC(token.Value);
                                _state = _record.ByteCount > 0
                                    ? LineState.Data
                                    : LineState.CRC;
                                return States.SameState;

                            case LineState.Data:
                                _record.LineCompleted = false;
                                _record.AddByte(token.Value);
                                _state = --_record.ByteCount > 0
                                   ? LineState.Data
                                   : LineState.CRC;
                                return States.SameState;

                            case LineState.CRC:
                                var crcRead = token.Value;
                                var crcCalc = _record.RunningCRC;
                                if (crcRead != crcCalc)
                                    throw new FileFormatException($"CRC mismatch. Got {crcRead:x2}, expected {crcCalc:x2}", token);
                                _record.LineCompleted = true;

                                switch((RecordTypes)_record.RecordType)
                                {
                                    case RecordTypes.Data:
                                        // Data
                                        // The byte count specifies number of data bytes in the record. 
                                        break;

                                    case RecordTypes.EndOfFile:
                                        // End Of File 
                                        // Must occur exactly once per file in the last record of the file.
                                        // The byte count is 00, the address field is typically 0000 and the
                                        // data field is omitted. 
                                        return States.EndOfFile;

                                    case RecordTypes.ExtendedSegmentAddress:
                                        // Extended Segment Address
                                        // The byte count is always 02, the address field (typically 0000) is
                                        // ignored and the data field contains a 16-bit segment base address.
                                        // This is multiplied by 16 and added to each subsequent data record
                                        // address to form the starting address for the data. This allows
                                        // addressing up to one megabyte of address space. 

                                        _record.AddressOffset = (uint)(((_record.Data.Dequeue() << 8) + _record.Data.Dequeue()) * 16);
                                        Reader.CurrentAddress = _record.AddressOffset;
                                        break;

                                    case RecordTypes.ExtendedLinearAddress:
                                        // Extended Linear Address 
                                        // Allows for 32 bit addressing (up to 4 GiB). The byte count is always
                                        // 02 and the address field is ignored (typically 0000). The two data
                                        // bytes (big endian) specify the upper 16 bits of the 32 bit absolute
                                        // address for all subsequent type 00 records; these upper address bits
                                        // apply until the next 04 record. The absolute address for a type 00
                                        // record is formed by combining the upper 16 address bits of the most
                                        // recent 04 record with the low 16 address bits of the 00 record. If a
                                        // type 00 record is not preceded by any type 04 records then its upper
                                        // 16 address bits default to 0000. 

                                        _record.AddressOffset = (uint)((_record.Data.Dequeue() << 24) + (_record.Data.Dequeue() << 16));
                                        Reader.CurrentAddress = _record.AddressOffset;
                                        break;

                                    case RecordTypes.StartSegmentAddress:
                                    case RecordTypes.StartLinearAddress:
                                        // Ignore the start addresses for this application
                                        break;
                                }

                                // Ditch the data if it's not a data record (else it will incorrectly be returned as data)
                                if ((RecordTypes)_record.RecordType != RecordTypes.Data)
                                    _record.Data.Clear();

                                return States.EndOfRecord;

                            default:
                                return States.SameState;

                        }

                    default:
                        throw new FileFormatException($"Unexpected {token.Type} token processing a line", token);
                }
            }

            protected override void InternalEnter()
            {
                _state = LineState.ByteCount;
                _record.ResetLine();
                Reader.CurrentAddress = _record.AddressOffset;
            }

            enum LineState
            {
                ByteCount,
                Address1,
                Address2,
                RecordType,
                Data,
                CRC,
                EndOfLine
            }

            LineState _state = LineState.ByteCount;
            Record _record;
        }

        internal class State_EndOfRecord : StateBase
        {
            public State_EndOfRecord(IntelHexReader reader) : base(reader, States.EndOfRecord)
            { }

            public override States ProcessToken(Token token)
            {
                return token.Type switch
                {
                    TokenType.EndOfLine => States.SameState,// Multiple end of lines, keep the same state
                    TokenType.EndOfStream => States.EndOfStream,// End of stream, nothing more to read
                    TokenType.StartCode => States.StartCode,
                    _ => throw new FileFormatException($"Unexpected {token.Type} token at the end of a line", token),
                };
            }
        }

        internal class State_EndOfFile : StateBase
        {
            public State_EndOfFile(IntelHexReader reader) : base(reader, States.EndOfRecord)
            { }

            public override States ProcessToken(Token token)
            {
                return token.Type switch
                {
                    TokenType.EndOfLine => States.SameState,// Multiple end of lines, keep the same state
                    TokenType.EndOfStream => States.EndOfStream,// End of stream, nothing more to read
                    _ => throw new FileFormatException($"Unexpected {token.Type} token at the end of a file", token),
                };
            }
        }
        #endregion States

        Record _record = new Record();
        readonly IEnumerator<Token> _tokens;
        StateBase _currentState;
        readonly Dictionary<States, StateBase> _states;
    }

    
    namespace IntelHex
    {
        public class Record
        {
            public byte ByteCount { get; set; } = 0;
            public byte RecordType { get; set; } = 0;
            public Queue<byte> Data { get; } = new ();
            public bool LineCompleted { get; set; } = false;
            public byte RunningCRC => (byte)(((byte)~_crc) + 1);
            public uint AddressOffset { get; internal set; } = 0;

            public void AddByte(byte data)
            {
                Data.Enqueue(data);
                AddToCRC(data);
            }

            public void AddToCRC(byte data)
            {
                _crc += data;
            }

            public void ResetLine()
            {
                ByteCount = 0;
                RecordType = 0;
                LineCompleted = true;
                Data.Clear();
                _crc = 0;
            }

            byte _crc = 0;
        }

        public class FileFormatException : ApplicationException
        {
            public FileFormatException(string message, int lineNumber, int colNumber) : base(message)
            {
                LineNumber = lineNumber;
                ColumnNumber = colNumber;
            }

            public FileFormatException(string message, Token token) : base(message)
            {
                LineNumber = token.LineNumber;
                ColumnNumber = token.ColumnNumber;
            }

            public int LineNumber { get; private set; }
            public int ColumnNumber { get; private set; }
        }

        #region Tokeniser
        public enum TokenType
        {
            StartCode,
            Hex,
            Invalid,
            EndOfLine,
            EndOfStream
        }

        [DebuggerDisplay("{Type, Value}")]
        public struct TokenTypeAndValue
        {
            public TokenTypeAndValue(TokenType type, byte value = 0)
            {
                Type = type;
                Value = value;
            }

            public TokenType Type { get; private set; }
            public byte Value { get; private set; }
        }


        public struct Token 
        {
            public Token(TokenType type, int lineNo, int colNo, byte value = 0)
            {
                TypeAndValue = new TokenTypeAndValue(type, value);
                LineNumber = lineNo;
                ColumnNumber = colNo;
            }

            public Token(TokenType type, byte value = 0)
            {
                TypeAndValue = new TokenTypeAndValue(type, value);
                LineNumber = 0;
                ColumnNumber = 0;
            }

            TokenTypeAndValue TypeAndValue { get; set; }
            public TokenType Type => TypeAndValue.Type;
            public byte Value => TypeAndValue.Value;
            public int LineNumber { get; private set; }
            public int ColumnNumber { get; private set; }

            public static explicit operator TokenTypeAndValue(Token token) => token.TypeAndValue;
        }

        public static class Tokeniser
        {
            // The spec says it should be an ASCII file, but we can probably let the StreamReader auto-detect,
            // and be a bit more lenient with the actual encoding
            public static IEnumerable<Token> GetTokens(this string hexData) => GetTokens(new MemoryStream(Encoding.UTF8.GetBytes(hexData)));

            public static IEnumerable<Token> GetTokens(this Stream hexData)
            {
                bool hasStartCode = false;
                byte digit = 0;
                bool isFirstDigit = true;
                const char START_CODE = ':';
                const char CR = '\r';
                const char LF = '\n';
                int cr = 0;
                int lf = 0;
                int lineNo = 1;
                int colNo = 0;

                using (var reader = new StreamReader(hexData))
                {
                    while(true)
                    {
                        var chr = reader.Read();

                        // If we have a newline, increment the newline character count
                        if (chr == CR || chr == LF)
                        {
                            // Line split in the middle of a byte
                            if (!isFirstDigit)
                            {
                                yield return new Token(TokenType.Invalid, lineNo, colNo, 0);
                                yield break;
                            }

                            if (chr == CR)
                                ++cr;
                            if (chr == LF)
                                ++lf;
                            continue;
                        }

                        // If it's not a newline char but we have a newline char count, yield the end-of-line token
                        else if (cr > 0 || lf > 0)
                        {
                            // Yield as many newlines as we have characters
                            for (int i = 0; i < Math.Max(cr, lf); i++)
                            {
                                yield return new Token(TokenType.EndOfLine, lineNo, colNo);
                                ++lineNo;
                                colNo = 0;
                            }

                            // Then, reset the newline char count
                            cr = 0;
                            lf = 0;
                            hasStartCode = false;
                        }

                        ++colNo;

                        // End of stream, exit the loop
                        if (chr == -1)
                        {
                            // End of stream in the middle of a byte
                            if (!isFirstDigit)
                            {
                                yield return new Token(TokenType.Invalid, lineNo, --colNo, 0);
                                yield break;
                            }
                            break;
                        }

                        // Look for the start code. 
                        if (chr == START_CODE)
                        {
                            // Start code in the middle of a byte
                            if (!isFirstDigit)
                            {
                                yield return new Token(TokenType.Invalid, lineNo, --colNo, 0);
                                yield break;
                            }
                            hasStartCode = true;
                            yield return new Token(TokenType.StartCode, lineNo, colNo);
                            continue;
                        }

                        if (hasStartCode)
                        {
                            // If we have the start code, everything that follows is data.
                            // If not, just discard until we get a start code

                            if (chr >= '0' && chr <= '9')
                                digit += (byte)(chr - '0');
                            else if (chr >= 'a' && chr <= 'f')
                                digit += (byte)(chr - 'a' + 10);
                            else if (chr >= 'A' && chr <= 'F')
                                digit += (byte)(chr - 'A' + 10);
                            else
                            {
                                yield return new Token(TokenType.Invalid, lineNo, colNo, 0);
                                yield break;
                            }

                            if (isFirstDigit)
                                digit <<= 4;
                            else
                            {
                                yield return new Token(TokenType.Hex, lineNo, colNo - 1, digit); // ColNo for first digit
                                digit = 0;
                            }

                            isFirstDigit = !isFirstDigit;
                        }

                    }

                    yield return new Token(TokenType.EndOfStream, lineNo, colNo);
                }
            }
        }
        #endregion // Tokeniser
    }


}
