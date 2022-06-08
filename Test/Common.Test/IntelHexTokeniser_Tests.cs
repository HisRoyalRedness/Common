using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HisRoyalRedness.com.IntelHex;

namespace HisRoyalRedness.com
{
    [TestClass]
    public class IntelHexToken_Tests
    {
        [TestMethod]
        public void IntelHexToken_EmptyStreamOnlyReturnsEndOfStream()
        {
            var tokens = IntelHex.Tokeniser.GetTokens("").ToList();

            tokens.AsTypeAndValue().Should().Equal(new[] {
                new TokenTypeAndValue(TokenType.EndOfStream)
            });
        }

        [TestMethod]
        public void IntelHexToken_RecogniseCR()
        {
            var tokens = IntelHex.Tokeniser.GetTokens("\r").ToList();

            tokens.AsTypeAndValue().Should().Equal(new[] {
                new TokenTypeAndValue(TokenType.EndOfLine),
                new TokenTypeAndValue(TokenType.EndOfStream)
            });
        }

        [TestMethod]
        public void IntelHexToken_RecogniseLR()
        {
            var tokens = IntelHex.Tokeniser.GetTokens("\n").ToList();

            tokens.AsTypeAndValue().Should().Equal(new[] {
                new TokenTypeAndValue(TokenType.EndOfLine),
                new TokenTypeAndValue(TokenType.EndOfStream)
            });
        }

        [TestMethod]
        public void IntelHexToken_RecogniseCRLF()
        {
            var tokens = IntelHex.Tokeniser.GetTokens("\r\n").ToList();

            tokens.AsTypeAndValue().Should().Equal(new[] {
                new TokenTypeAndValue(TokenType.EndOfLine),
                new TokenTypeAndValue(TokenType.EndOfStream)
            });
        }

        [TestMethod]
        public void IntelHexToken_RecogniseMultipleCombinationCRLF()
        {
            var tokens = IntelHex.Tokeniser.GetTokens("\r\n\n\r\r\r\n\n\r").ToList();

            tokens.AsTypeAndValue().Should().Equal(new[] {
                new TokenTypeAndValue(TokenType.EndOfLine),
                new TokenTypeAndValue(TokenType.EndOfLine),
                new TokenTypeAndValue(TokenType.EndOfLine),
                new TokenTypeAndValue(TokenType.EndOfLine),
                new TokenTypeAndValue(TokenType.EndOfLine),
                new TokenTypeAndValue(TokenType.EndOfStream)
            });
        }

        [TestMethod]
        public void IntelHexToken_IgnoreContentInFrontOfStartCode()
        {
            var tokens = IntelHex.Tokeniser.GetTokens("abc 123 #$% :").ToList();

            tokens.AsTypeAndValue().Should().Equal(new[] {
                new TokenTypeAndValue(TokenType.StartCode),
                new TokenTypeAndValue(TokenType.EndOfStream)
            });
        }

        [TestMethod]
        public void IntelHexToken_IgnoreContentInFrontOfStartCodeOnMultipleLines()
        {
            var tokens = IntelHex.Tokeniser.GetTokens("abc 123 #$% :\n2323:").ToList();

            tokens.AsTypeAndValue().Should().Equal(new[] {
                new TokenTypeAndValue(TokenType.StartCode),
                new TokenTypeAndValue(TokenType.EndOfLine),
                new TokenTypeAndValue(TokenType.StartCode),
                new TokenTypeAndValue(TokenType.EndOfStream)
            });
        }

        [TestMethod]
        public void IntelHexToken_ParseDigits()
        {
            var tokens = IntelHex.Tokeniser.GetTokens(":0123456789aBCdeF").ToList();

            tokens.AsTypeAndValue().Should().Equal(new[] {
                new TokenTypeAndValue(TokenType.StartCode),
                new TokenTypeAndValue(TokenType.Hex, 0x01),
                new TokenTypeAndValue(TokenType.Hex, 0x23),
                new TokenTypeAndValue(TokenType.Hex, 0x45),
                new TokenTypeAndValue(TokenType.Hex, 0x67),
                new TokenTypeAndValue(TokenType.Hex, 0x89),
                new TokenTypeAndValue(TokenType.Hex, 0xab),
                new TokenTypeAndValue(TokenType.Hex, 0xcd),
                new TokenTypeAndValue(TokenType.Hex, 0xef),
                new TokenTypeAndValue(TokenType.EndOfStream)
            });

            tokens[4].Should().Be(new Token(type: TokenType.Hex, lineNo: 1, colNo: 8, value: 0x67));
        }

        [TestMethod]
        public void IntelHexToken_ParseInvalidDigit()
        {
            var tokens = IntelHex.Tokeniser.GetTokens(":0123456g89aBCdeF").ToList();

            tokens.AsTypeAndValue().Should().Equal(new[] {
                new TokenTypeAndValue(TokenType.StartCode),
                new TokenTypeAndValue(TokenType.Hex, 0x01),
                new TokenTypeAndValue(TokenType.Hex, 0x23),
                new TokenTypeAndValue(TokenType.Hex, 0x45),
                new TokenTypeAndValue(TokenType.Invalid),
            });

            tokens[4].Should().Be(new Token(type: TokenType.Invalid, lineNo: 1, colNo: 9, value: 0));
        }

        [TestMethod]
        public void IntelHexToken_SplitByteOverNewLine()
        {
            var tokens = IntelHex.Tokeniser.GetTokens(":f\rf").ToList();

            tokens.AsTypeAndValue().Should().Equal(new[] {
                new TokenTypeAndValue(TokenType.StartCode),
                new TokenTypeAndValue(TokenType.Invalid)
            });

            tokens[1].Should().Be(new Token(type: TokenType.Invalid, lineNo: 1, colNo: 2, value: 0));
        }

        [TestMethod]
        public void IntelHexToken_SplitByteOverStartCode()
        {
            var tokens = IntelHex.Tokeniser.GetTokens("  :f:f").ToList();

            tokens.AsTypeAndValue().Should().Equal(new[] {
                new TokenTypeAndValue(TokenType.StartCode),
                new TokenTypeAndValue(TokenType.Invalid)
            });

            tokens[1].Should().Be(new Token(type: TokenType.Invalid, lineNo: 1, colNo: 4, value: 0));
        }

        [TestMethod]
        public void IntelHexToken_SplitByteOverEndOfStream()
        {
            var tokens = IntelHex.Tokeniser.GetTokens("  :f").ToList();

            tokens.AsTypeAndValue().Should().Equal(new[] {
                new TokenTypeAndValue(TokenType.StartCode),
                new TokenTypeAndValue(TokenType.Invalid)
            });

            tokens[1].Should().Be(new Token(type: TokenType.Invalid, lineNo: 1, colNo: 4, value: 0));
        }

        [TestMethod]
        public void IntelHexToken_CheckForInvalidBytes()
        {
            for (ushort b = 0; b < 256; ++b)
            {
                var tokens = IntelHex.Tokeniser.GetTokens(":{b}").ToList();

                tokens.AsTypeAndValue().Should().Equal(new[] {
                    new TokenTypeAndValue(TokenType.StartCode),
                    new TokenTypeAndValue(TokenType.Invalid)
                });
            }
        }
    }


    internal static class IntelHexExtensions
    {
        public static Stream ToStream(this string data)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(data);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static IEnumerable<TokenTypeAndValue> AsTypeAndValue(this IEnumerable<Token> token) => token.Select(t => (TokenTypeAndValue)t);

        public static bool IsValidIntelHexChar(this byte b)
        {
            // Newline and start code are known valid
            if (b == '\r' || b == '\n' || b == ':')
                return true;

            // So are hex chars
            if ((b >= '0' && b <= '9') ||
                (b >= 'a' && b <= 'f') ||
                (b >= 'A' && b <= 'F'))
                return true;

            return false;
        }
    }
}
