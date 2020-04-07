using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
    A text writer that automatically creates new log files
    once the current file exceeds a specified maximum size.

    Keith Fletcher
    Oct 2017

    This file is Unlicensed.
    See the foot of the file, or refer to <http://unlicense.org>
*/

namespace HisRoyalRedness.com
{
    /// <summary>
    /// A TextWriter that automatically starts logging to a new
    /// log file once the current log file exceeds a specified
    /// maximum size.
    /// </summary>
    public class RollableLogWriter : TextWriter
    {
        #region Constructors
        /// <summary>
        /// Create a new <see cref="RollableLogWriter"/> with the default
        /// extension (.log), file name generator and encoding (UTF-8).
        /// </summary>
        public RollableLogWriter()
            : this(string.Empty, null, DEFAULT_EXTENSION, _defaultEncoding)
        { }

        /// <summary>
        /// Create a new <see cref="RollableLogWriter"/> with the default
        /// extension (.log), file name generator and encoding (UTF-8)
        /// </summary>
        /// <param name="basePath">Path to save log files to. Relative paths
        /// with be calculated relative to <paramref name="basePath"/>.</param>
        public RollableLogWriter(string basePath)
            : this(basePath, null, DEFAULT_EXTENSION, _defaultEncoding)
        { }

        /// <summary>
        /// Create a new <see cref="RollableLogWriter"/> with the default
        /// extension (.log) and encoding (UTF-8).
        /// </summary>
        /// <param name="basePath">Path to save log files to. Relative paths
        /// with be calculated relative to <paramref name="basePath"/>.</param>
        /// <param name="fileNameGenerator">A Func that accepts a base path
        /// and returns an absolute file name for the new log file.</param>
        public RollableLogWriter(string basePath, Func<string, string> fileNameGenerator)
            : this(basePath, fileNameGenerator, DEFAULT_EXTENSION, _defaultEncoding)
        { }

        /// <summary>
        /// Create a new <see cref="RollableLogWriter"/> with the default encoding (UTF-8).
        /// </summary>
        /// <param name="basePath">Path to save log files to. Relative paths
        /// with be calculated relative to <paramref name="basePath"/>.</param>
        /// <param name="fileNameGenerator">A Func that accepts a base path
        /// and returns an absolute file name for the new log file.</param>
        /// <param name="fileExtension">The extensions for the log files.</param>
        public RollableLogWriter(string basePath, Func<string, string> fileNameGenerator, string fileExtension)
            : this(basePath, fileNameGenerator, DEFAULT_EXTENSION, _defaultEncoding)
        { }

        /// <summary>
        /// Create a new <see cref="RollableLogWriter"/>
        /// </summary>
        /// <param name="basePath">Path to save log files to. Relative paths
        /// with be calculated relative to <paramref name="basePath"/>.</param>
        /// <param name="fileNameGenerator">A Func that accepts a base path
        /// and returns an absolute file name for the new log file.</param>
        /// <param name="fileExtension">The extensions for the log files.</param>
        /// <param name="encoding">The encoding of the log file.</param>
        public RollableLogWriter(string basePath, Func<string, string> fileNameGenerator, string fileExtension, Encoding encoding)
        {
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            _newLine = _encoding.GetBytes(Environment.NewLine);

            if (string.IsNullOrWhiteSpace(basePath))
                _basePath = string.Empty;
            else
            {
                _basePath = basePath;
                if (!Directory.Exists(_basePath))
                    Directory.CreateDirectory(_basePath);
            }

            FileExtension = fileExtension;

            // Func to generate the filename of the next log file
            _fileNameGenerator = fileNameGenerator ?? (baseDir =>
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd HHmmss");
                var path = string.Empty;
                var fileNum = -1;
                do
                {
                    // If there are multiple files with the same timestamp
                    // (can happen if we're writing a LOT of data), then append
                    // a sequential number to the end of the filename.
                    var fileName = timestamp;
                    if (++fileNum > 0)
                        fileName += $".{(fileNum.ToString("D2"))}";
                    path = Path.GetFullPath(string.IsNullOrWhiteSpace(BasePath)
                        ? fileName + FileExtension
                        : Path.Combine(BasePath, fileName + FileExtension));
                } while (File.Exists(path));
                return path;
            });
        }
        #endregion Constructors

        /// <summary>
        /// The maximum log file size. If the current log file exceeds this limit,
        /// a new log file is created and because the current log file. The default
        /// <see cref="MaxLogFileSize"/> is 10MB.
        /// </summary>
        public int MaxLogFileSize
        {
            get => _maxLogSize;
            set
            {
                if (MaxLogFileSize < 0)
                    throw new ArgumentOutOfRangeException(nameof(MaxLogFileSize), $"{nameof(MaxLogFileSize)} must be zero or greater.");
                _maxLogSize = value;
            }
        }

        public bool Append => false;
        /// <summary>
        /// Path to save log files to. Relative paths
        /// with be calculated relative to <see cref="BasePath"/>.
        /// </summary>
        public string BasePath => _basePath;
        public override Encoding Encoding => _encoding;
        /// <summary>
        /// Returns the absolute file name of the current log file
        /// </summary>
        public string FileName => _fileName;
        /// <summary>
        /// Gets or sets the extension to use when generating log file names.
        /// </summary>
        public string FileExtension { get; set; } = DEFAULT_EXTENSION;
        public bool AutoFlush { get; set; } = false;

        /// <summary>
        /// A string header to include at the head of each log file.
        /// </summary>
        public string HeaderLine { get; set; } = string.Empty;
        /// <summary>
        /// A string footer to include at the foot of each log file.
        /// </summary>
        public string FooterLine { get; set; } = string.Empty;

        /// <summary>
        /// Manually roll to a new log file, regardless of the current log file size.
        /// </summary>
        public void RollLogFile()
        {
            _semaphore.Wait();
            try
            {
                CreateNewLogFile();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Manually roll to a new log file, regardless of the current log file size.
        /// </summary>
        public Task RollLogFileAsync() => RollLogFileAsync(CancellationToken.None);

        /// <summary>
        /// Manually roll to a new log file, regardless of the current log file size.
        /// </summary>
        public async Task RollLogFileAsync(CancellationToken token)
        {
            await _semaphore.WaitAsync(token);
            try
            {
                await CreateNewLogFileAsync(token);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        #region Write overrides
        public override void Write(char value) => WriteInternal(ToByteArray(value));
        public override void Write(char[] buffer) => WriteInternal(ToByteArray(buffer));
        public override void Write(char[] buffer, int index, int count) => WriteInternal(ToByteArray(buffer, index, count));
        public override void Write(string value) => WriteInternal(ToByteArray(value));
        public override void WriteLine(string value) => WriteInternal(ToByteArray(value), true);

        public override Task WriteAsync(char value) => WriteInternalAsync(ToByteArray(value));
        public override Task WriteAsync(char[] buffer, int index, int count) => WriteInternalAsync(ToByteArray(buffer, index, count));
        public override Task WriteAsync(string value) => WriteInternalAsync(ToByteArray(value));
        public override Task WriteLineAsync() => WriteInternalAsync(new byte[] { }, true);
        public override Task WriteLineAsync(char value) => WriteInternalAsync(ToByteArray(value), true);
        public override Task WriteLineAsync(char[] buffer, int index, int count) => WriteInternalAsync(ToByteArray(buffer, index, count), true);
        public override Task WriteLineAsync(string value) => WriteInternalAsync(ToByteArray(value), true);

        public Task WriteAsync(char value, CancellationToken token) => WriteInternalAsync(ToByteArray(value), token);
        public Task WriteAsync(char[] buffer, int index, int count, CancellationToken token) => WriteInternalAsync(ToByteArray(buffer, index, count), token);
        public Task WriteAsync(string value, CancellationToken token) => WriteInternalAsync(ToByteArray(value), token);
        public Task WriteLineAsync(CancellationToken token) => WriteInternalAsync(new byte[] { }, token, true);
        public Task WriteLineAsync(char value, CancellationToken token) => WriteInternalAsync(ToByteArray(value), token, true);
        public Task WriteLineAsync(char[] buffer, int index, int count, CancellationToken token) => WriteInternalAsync(ToByteArray(buffer, index, count), token, true);
        public Task WriteLineAsync(string value, CancellationToken token) => WriteInternalAsync(ToByteArray(value), token, true);
        #endregion Write overrides

        #region CreateNewLogFile
        void CreateNewLogFile()
        {
            if (_baseStream != null)
            {
                if (!string.IsNullOrWhiteSpace(FooterLine))
                    WriteLine(FooterLine);
                _baseStream.Flush();
                _baseStream.Close();
                _baseStream = null;
                _fileName = string.Empty;
            }
            var fileName = _fileNameGenerator(BasePath);
            _baseStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read, 1024 * 1024, FileOptions.WriteThrough);
            _fileName = fileName;
            _currentSize = 0;
            if (!string.IsNullOrWhiteSpace(HeaderLine))
                WriteInternalNoLock(ToByteArray(HeaderLine), true); // We should already be inside a semaphore lock
        }

        Task CreateNewLogFileAsync() => CreateNewLogFileAsync(CancellationToken.None);
        async Task CreateNewLogFileAsync(CancellationToken token)
        {
            if (_baseStream != null)
            {
                if (!string.IsNullOrWhiteSpace(FooterLine))
                    await WriteLineAsync(FooterLine, token);
                await _baseStream.FlushAsync(token);
                _baseStream.Close();
                _baseStream = null;
                _fileName = string.Empty;
            }
            var fileName = _fileNameGenerator(BasePath);
            _baseStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read, 1024 * 1024, FileOptions.WriteThrough);
            _fileName = fileName;
            _currentSize = 0;
            if (!string.IsNullOrWhiteSpace(HeaderLine))
                await WriteLineAsync(HeaderLine, token);
        }
        #endregion CreateNewLogFile

        protected override void Dispose(bool disposing)
        {
            if (_baseStream != null)
                _baseStream.Dispose();
            _semaphore.Dispose();
            _baseStream = null;
            _fileName = string.Empty;
            base.Dispose(disposing);
        }

        #region Byte array conversions
        [DebuggerStepThrough]
        byte[] ToByteArray(char value) => ToByteArray(new[] { value });
        byte[] ToByteArray(char[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            return buffer.Length == 0
                ? new byte[] { }
                : _encoding.GetBytes(buffer);
        }

        [DebuggerStepThrough]
        byte[] ToByteArray(char[] buffer, int index, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (MaxLogFileSize < 0)
                throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} must be zero or greater.");
            if (MaxLogFileSize < 0)
                throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} must be zero or greater.");
            if (buffer.Length - index < count)
                throw new ArgumentException($"{nameof(index)} and {nameof(count)} were out of bounds for the array or {nameof(count)} is greater than the number of elements from {nameof(index)} to the end of the source collection.");

            return count == 0
                ? new byte[] { }
                : _encoding.GetBytes(buffer, index, count);
        }

        [DebuggerStepThrough]
        byte[] ToByteArray(string buffer)
            => string.IsNullOrEmpty(buffer)
                ? new byte[] { }
                : _encoding.GetBytes(buffer);
        #endregion Byte array conversions

        #region WriteInternal
        //[DebuggerStepThrough]
        int WriteInternal(byte[] buffer, bool newLine = false)
        {
            var bytesWritten = 0;
            if (buffer.Length > 0 || newLine)
            {
                _semaphore.Wait();
                try
                {
                    bytesWritten = WriteInternalNoLock(buffer, newLine);
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            return 0;
        }

        int WriteInternalNoLock(byte[] buffer, bool newLine = false)
        {
            var bytesWritten = 0;
            if (buffer.Length > 0 || newLine)
            {
                if (_currentSize + buffer.Length > MaxLogFileSize || _baseStream == null)
                    CreateNewLogFile();
                if (buffer.Length > 0)
                {
                    _baseStream.Write(buffer, 0, buffer.Length);
                    bytesWritten = buffer.Length;
                }
                if (newLine)
                {
                    _baseStream.Write(_newLine, 0, _newLine.Length);
                    bytesWritten += _newLine.Length;
                }
                if (AutoFlush)
                    _baseStream.Flush();
                _currentSize += bytesWritten;
            }
            return bytesWritten;
        }

        [DebuggerStepThrough]
        Task<int> WriteInternalAsync(byte[] buffer, bool newLine = false) => WriteInternalAsync(buffer, CancellationToken.None, newLine);
        [DebuggerStepThrough]
        async Task<int> WriteInternalAsync(byte[] buffer, CancellationToken token, bool newLine = false)
        {
            var bytesWritten = 0;
            if (buffer.Length > 0 || newLine)
            {
                await _semaphore.WaitAsync(token);
                try
                {
                    if (_currentSize + buffer.Length > MaxLogFileSize || _baseStream == null)
                        await CreateNewLogFileAsync(token);
                    if (buffer.Length > 0)
                    {
                        await _baseStream.WriteAsync(buffer, 0, buffer.Length, token);
                        bytesWritten = buffer.Length;
                    }
                    if (newLine)
                    {
                        await _baseStream.WriteAsync(_newLine, 0, _newLine.Length, token);
                        bytesWritten += _newLine.Length;
                    }
                    if (AutoFlush)
                        await _baseStream.FlushAsync(token);
                    _currentSize += bytesWritten;
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            return bytesWritten;
        }
        #endregion WriteInternal

        const int DEFAULT_MAX_SIZE = 1024 * 1024 * 10; // 10MB max log file
        const string DEFAULT_EXTENSION = ".log";

        readonly static Encoding _defaultEncoding = Encoding.UTF8;
        readonly Func<string, string> _fileNameGenerator;
        readonly string _basePath;
        readonly Encoding _encoding = Encoding.UTF8;
        string _fileName = string.Empty;
        readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        readonly byte[] _newLine;
        FileStream _baseStream = null;
        int _maxLogSize = DEFAULT_MAX_SIZE;
        int _currentSize = 0;
    }
}

/*
This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org>
*/
