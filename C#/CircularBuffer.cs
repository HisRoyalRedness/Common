using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    A circular buffer implementation for C#
    https://en.wikipedia.org/wiki/Circular_buffer
    
        The buffer can be initialised with OverWriting either enabled or
        disabled (disabled by default). If it is enabled, an exception 
        will be thrown if the buffer capacity is exceeded, otherwise it 
        will simply drop as many elements off the back as needed in order 
        to stay within the capacity limit.

        Data is added singularly with Add() and removed singulary with 
        Remove(). The usual ICollection methods all work as expected.

        Read() and Write() have been provided to extract or add blocks
        of data respectively.

    Keith Fletcher
    Mar 2016
    
    This file is Unlicensed. 
    See the foot of the file, or refer to <http://unlicense.org>
*/

namespace fletcher.org
{
    public class CircularBuffer<T> : ICollection<T>
    {
        #region Constructors
        /// <summary>
        /// Create a new instance of <see cref="CircularBuffer{T}"/>,
        /// with a default <see cref="Capacity"/> of 1024. 
        /// <see cref="CanOverwrite"/> defaults to false.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is not larger than 0.</exception>
        public CircularBuffer()
            : this(DEFAULT_CAPACITY, false)
        { }

        /// <summary>
        /// Create a new instance of <see cref="CircularBuffer{T}"/>,
        /// with the specified <paramref name="capacity"/>. 
        /// <see cref="CanOverwrite"/> defaults to false.
        /// </summary>
        /// <param name="capacity">The maximum number of elements that the 
        /// <see cref="CircularBuffer{T}"/> can hold</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is not larger than 0.</exception>
        public CircularBuffer(int capacity)
            : this(capacity, false)
        { }

        /// <summary>
        /// Create a new instance of <see cref="CircularBuffer{T}"/>,
        /// with the specified <paramref name="capacity"/>. 
        /// </summary>
        /// <param name="capacity">The maximum number of elements that the 
        /// <see cref="CircularBuffer{T}"/> can hold</param>
        /// <param name="overwrite">If <paramref name="overwrite"/> is true, an <see cref="ArgumentException"/>
        /// is thrown if you exceed the buffer size. When <paramref name="overwrite"/>
        /// is false, newer data overwrites the older elements in the buffer.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is not larger than 0.</exception>
        public CircularBuffer(int capacity, bool overwrite = false)
        {
            Contract.Requires<ArgumentOutOfRangeException>(capacity >= 0, $"{nameof(capacity)} must be larger than zero.");
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), $"{nameof(capacity)} must be larger than zero.");

            _overwrite = overwrite;
            _capacity = capacity;
            _buffer = new T[_capacity];
        }
        #endregion Constructors

        #region ICollection<T> implementation
        /// <summary>
        /// Returns the actual number of elements in the <see cref="CircularBuffer{T}"/>
        /// </summary>
        /// <returns>The actual number of elements in the <see cref="CircularBuffer{T}"/></returns>
        public int Count =>
            _readIndex == _writeIndex
                ? (_isFull ? _capacity : 0)
                : (_readIndex > _writeIndex ? _capacity : 0) + _writeIndex - _readIndex;

        /// <summary>
        /// Always returns false. The <see cref="CircularBuffer{T}"/>
        /// cannot be made read-only.
        /// </summary>
        /// <returns>False</returns>
        public bool IsReadOnly => false;

        /// <summary>
        /// Add <paramref name="item"/> to the <see cref="CircularBuffer{T}"/>.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The buffer is already full and <see cref="CanOverwrite"/> is false.</exception>
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

        /// <summary>
        /// Removes all elements from teh buffer.
        /// </summary>
        public void Clear() 
        {
            _readIndex = 0;
            _writeIndex = 0;
        }

        public bool Contains(T item)
        {
            var index = _readIndex;
            for (var i = 0; i < Count; ++i)
                if (_buffer[IndexInc(ref index)] == item)
                    return true;

                //return ;

        }

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

        public bool CanOverwrite => _overwrite;

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
