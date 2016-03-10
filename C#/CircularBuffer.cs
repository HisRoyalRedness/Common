using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallagher.Utilities
{
    public class CircularBuffer<T> : ICollection<T>
    {
        #region Constructors
        public CircularBuffer()
            : this(DEFAULT_CAPACITY)
        { }

        public CircularBuffer(int capacity, bool overwrite = false)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), $"{nameof(capacity)} must be larger than zero.");
            _overwrite = overwrite;
            _capacity = capacity;
            _buffer = new T[_capacity];
        }
        #endregion Constructors

        #region ICollection<T> implementation
        public int Count =>
            _readIndex == _writeIndex
                ? (_isFull ? _capacity : 0)
                : (_readIndex > _writeIndex ? _capacity : 0) + _writeIndex - _readIndex;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (_isFull && !_overwrite)
                throw new InvalidOperationException($"Cannot {nameof(Add)} another {nameof(item)}. The {nameof(CircularBuffer<T>)} is full.");

            _buffer[IndexInc(ref _writeIndex)] = item;
            if (_isFull)
                _readIndex = _writeIndex;
            else
                _isFull = (_writeIndex == _readIndex);
        }

        public void Clear() 
        {
            _readIndex = 0;
            _writeIndex = 0;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public bool Contains(T item)
        { throw new NotImplementedException(); }

        /// <summary>
        /// Not implemented
        /// </summary>
        public bool Remove(T item)
        { throw new NotImplementedException(); }

        public void CopyTo(T[] data, int offset) => CopyTo(data, offset, Count);        

        public void CopyTo(T[] data, int offset, int length)
        {
            if (length == 0)
                return;
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} must be larger or equal to zero.");
            if (data.Length < length)
                throw new ArgumentException($"Not enough space in {nameof(data)}.");
            if (data.Length - offset < length)
                throw new ArgumentException($"Not enough space in {nameof(data)} with the provided {nameof(offset)}.");

            if (_readIndex < _writeIndex)
                Array.Copy(_buffer, _readIndex, data, offset, length);
            else
            {
                var firstPortion = _capacity - _readIndex;
                if (firstPortion > length)
                    firstPortion = length;
                Array.Copy(_buffer, _readIndex, data, offset, firstPortion);
                if (length - firstPortion > 0)
                    Array.Copy(_buffer, 0, data, offset + firstPortion, length - firstPortion);
            }
        }

        /// <summary>
        /// Each iteration will read from the current read index,
        /// while Count is greater than zero.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            while (Count > 0)
                yield return Remove();
        }

        /// <summary>
        /// Each iteration will read from the current read index,
        /// while Count is greater than zero.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            while (Count > 0)
                yield return Remove();
        }
        #endregion ICollection<T> implementation

        /// <summary>
        /// Remove a single <typeparamref name="T"/> instance from the buffer and return it.
        /// </summary>
        /// <returns>The top-most <typeparamref name="T"/> instance in the buffer </returns>
        public T Remove()
        {
            if (Count == 0)
                throw new InvalidOperationException($"Cannot {nameof(Remove)}. The {nameof(CircularBuffer<T>)} is empty.");

            _isFull = false;
            return _buffer[IndexInc(ref _readIndex)];
        }

        public int Read(T[] data, int offset, int length)
        {
            var len = length > Count ? Count : length;
            CopyTo(data, offset, len);
            _readIndex += len;
            if (len == Count)
                _readIndex = _writeIndex;
            else if (_readIndex >= _capacity)
                _readIndex -= _capacity;
            _isFull = false;
            return len;
        }

        /// <summary>
        /// Extract <paramref name="length"/> items from <paramref name="data"/>,
        /// starting at <paramref name="offset"/>, then add them to the 
        /// <see cref="CircularBuffer{T}"/> instance.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
        /// <exception cref="System.ArgumentException"><paramref name="data"/> is smaller than <paramref name="length"/> considering the <paramref name="offset"/> given.</exception>
        /// <exception cref="System.ArgumentException">There is not enough space to for <paramref name="length"/>  items and overwriting is not permitted.</exception>
        public void Write(T[] data, int offset, int length)
        {
            if (length == 0)
                return;
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} must be larger or equal to zero.");
            if (data.Length < length)
                throw new ArgumentException($"{nameof(data)} is smaller than {nameof(length)}.");
            if (data.Length - offset < length)
                throw new ArgumentException($"{nameof(data)} is smaller than {nameof(length)} with the provided {nameof(offset)}.");
            if (length + Count > _capacity && !_overwrite)
                throw new ArgumentException($"Not enough space in the {nameof(CircularBuffer<T>)} for the {nameof(length)} given.");

            _isFull = length + Count >= _capacity;

            if (length > _capacity)
            {
                offset += length - _capacity;
                length = _capacity;
            }

            if (_writeIndex + length >= _capacity)
            {
                var firstPortion = _capacity - _writeIndex;
                Array.Copy(data, offset, _buffer, _writeIndex, firstPortion);
                _writeIndex += firstPortion;
                if (_writeIndex == _capacity)
                    _writeIndex = 0;
                length -= firstPortion;
                offset += firstPortion;
            }

            if (length > 0)
            {
                Array.Copy(data, offset, _buffer, _writeIndex, length);
                _writeIndex += length;
            }

            if (_isFull)
                _readIndex = _writeIndex;
        }

        public int Capacity => _capacity;

        #region Index manipulation
        /// <summary>
        /// ++index
        /// </summary>
        int IncIndex(ref int index)
        {
            if (++index >= _capacity)
                index = 0;
            return index;
        }

        /// <summary>
        /// index++
        /// </summary>
        int IndexInc(ref int index)
        {
            var ind = index;
            if (++index >= _capacity)
                index = 0;
            return ind;
        }
        #endregion

        const int DEFAULT_CAPACITY = 1024;

        bool _overwrite = false;
        bool _isFull = false;
        int _capacity;
        T[] _buffer;
        int _readIndex = 0;
        int _writeIndex = 0;
    }

    public class CircularByteBuffer : CircularBuffer<byte>
    {
        public CircularByteBuffer()
            : base()
        { }

        public CircularByteBuffer(int capacity, bool overwrite = false)
            : base(capacity, overwrite)
        { }

        // serial.Read(buffer, writeOffset, bytesToRead);

        //public static implicit operator byte[] (CircularByteBuffer buffer)
        //{
        //    return new byte[0];
        //}

        //public static implicit operator CircularByteBuffer(byte[] buffer)
        //{
        //    return new CircularByteBuffer();
        //}
    }

    public static class CircularBufferExtensions
    {
        public static int Read(this SerialPort serial, CircularByteBuffer buffer, int offset, int count)
        {
            var data = new byte[count];
            var bytesRead = serial.Read(data, 0, data.Length);
            if (bytesRead > 0)
                buffer.Write(data, 0, bytesRead);
            return bytesRead;
        }

        public static int Read(this SerialPort serial, CircularByteBuffer buffer, int offset, int count, Stream stream)
        {
            var data = new byte[count];
            var bytesRead = serial.Read(data, 0, data.Length);
            if (bytesRead > 0)
            {
                buffer.Write(data, 0, bytesRead);
                stream.Write(data, 0, bytesRead);
            }
            return bytesRead;
        }
    }
}