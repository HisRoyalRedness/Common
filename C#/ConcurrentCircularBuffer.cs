using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

/*
    File description (short)

        File description (long)

    Keith Fletcher
    Sep 2019

    This file is Unlicensed.
    See the foot of the file, or refer to <http://unlicense.org>
*/

namespace HisRoyalRedness.com
{
    public class ConcurrentCircularBuffer<T> : ICollection<T>
    {
        #region Construction
        public ConcurrentCircularBuffer()
            : this(Enumerable.Empty<T>(), DEFAULT_CAPACITY, DEFAULT_OVERWRITE)
        { }

        public ConcurrentCircularBuffer(int capacity)
            : this(Enumerable.Empty<T>(), capacity, DEFAULT_OVERWRITE)
        { }

        public ConcurrentCircularBuffer(bool overwrite)
            : this(Enumerable.Empty<T>(), DEFAULT_CAPACITY, overwrite)
        { }

        public ConcurrentCircularBuffer(int capacity, bool overwrite)
            : this(Enumerable.Empty<T>(), capacity, overwrite)
        { }

        public ConcurrentCircularBuffer(IEnumerable<T> initialItems)
            : this(initialItems, DEFAULT_CAPACITY, DEFAULT_OVERWRITE)
        { }

        public ConcurrentCircularBuffer(IEnumerable<T> initialItems, int capacity)
            : this(initialItems, capacity, DEFAULT_OVERWRITE)
        { }

        public ConcurrentCircularBuffer(IEnumerable<T> initialItems, bool overwrite)
            : this(initialItems, DEFAULT_CAPACITY, overwrite)
        { }

        public ConcurrentCircularBuffer(IEnumerable<T> initialItems, int capacity, bool overwrite)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(capacity)} must be larger than 0.");

            if (initialItems == null)
                throw new ArgumentNullException($"{nameof(initialItems)} cannot be null.");

            Capacity = capacity;
            Overwrite = overwrite;

            _buffer = new T[Capacity];
            _memBuffer = new Memory<T>(_buffer);

            foreach (var item in initialItems)
            {
                _buffer[IndexInc(ref _writeIndex)] = item;
                if (_isFull)
                {
                    if (!Overwrite)
                        throw new ArgumentOutOfRangeException($"The number of {nameof(initialItems)} exceeds the {nameof(Capacity)} and {nameof(Overwrite)} is not permitted.");
                    _readIndex = _writeIndex;
                }
                else if (_writeIndex == 0)
                    _isFull = true;
            }

            // Create a binary semaphore. If we have initial items in the collection,
            // set it to signalled so that the first BlockedRead or BlockedRemove
            // will return immediately. 
            _signal = new SemaphoreSlim(_CountNotLocked > 0 ? 1 : 0, 1);
        }
        #endregion Construction

        #region IEnumerable<T> and IEnumerable implementation
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion IEnumerable<T> and IEnumerable implementation

        #region Add
        public void Add(T item) => Add(item, Overwrite);

        public void Add(T item, bool overwrite)
        {
            lock (_lockObj)
            { 
                _AddNotLocked(item, overwrite);
            } 
        }

        void _AddNotLocked(T item, bool overwrite)
        {
            if (_isFull && !overwrite)
                throw new InvalidOperationException($"Cannot {nameof(Add)} another {nameof(item)}. The {nameof(ConcurrentCircularBuffer<T>)} is full.");

            _buffer[IndexInc(ref _writeIndex)] = item;
            if (_isFull)
                _readIndex = _writeIndex;
            else
                _isFull = (_writeIndex == _readIndex);

            if (_signal.CurrentCount == 0)
                _signal.Release(1);
        }
        #endregion Add

        #region Remove
        public bool Remove(T item) => throw new NotSupportedException("Removing by item is not supported");

        public T Remove()
        {
            lock (_lockObj)
            {
                return _RemoveNotLocked();
            }
        }

        public int Remove(int length)
        {
            lock (_lockObj)
            {
                return _RemoveNotLocked(length);
            }
        }

        T _RemoveNotLocked()
        {
            if (_CountNotLocked == 0)
                throw new InvalidOperationException($"Cannot {nameof(Remove)}. The {nameof(ConcurrentCircularBuffer<T>)} is empty.");

            _isFull = false;
            return _buffer[IndexInc(ref _readIndex)];
        }

        int _RemoveNotLocked(int length)
        {
            var len = length > _CountNotLocked ? _CountNotLocked : length;
            if (len == _CountNotLocked)
                _ClearNotLocked();
            else
            {
                _readIndex += len;
                if (_readIndex >= Capacity)
                    _readIndex -= Capacity;
                _isFull = false;
            }
            return len;
        }
        #endregion Remove

        #region Clear
        public void Clear()
        {
            lock (_lockObj)
                _ClearNotLocked();
        }

        void _ClearNotLocked()
        {
            _readIndex = 0;
            _writeIndex = 0;
            _isFull = false;
        }
        #endregion Clear

        #region Contains
        public bool Contains(T item)
        {
            lock (_lockObj)
            {
                for (var i = 0; i < _CountNotLocked; ++i)
                    if (item.Equals(_AtIndexNotLocked(i)))
                        return true;
                return false;
            }
        }
        #endregion Contains

        #region Copy
        public void CopyTo(T[] array, int offset) => CopyTo(array, offset, Count);

        public void CopyTo(T[] array, int offset, int length)
        {
            if (length == 0)
                return;
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} must be larger or equal to zero.");
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} must be larger or equal to zero.");
            if (array.Length < length)
                throw new InvalidOperationException($"Not enough space in {nameof(array)}.");
            if (array.Length - offset < length)
                throw new InvalidOperationException($"Not enough space in {nameof(array)} with the provided {nameof(offset)}.");

            lock (_lockObj)
                _CopyToNotLocked(array, offset, length);
        }

        public int CopyTo(Memory<T> memory)
        {
            lock (_lockObj)
                return _CopyToUnlocked(memory);
        }

        void _CopyToNotLocked(T[] array, int offset, int length)
        {
            if (_CountNotLocked == 0 || length == 0)
                return;

            if (_readIndex < _writeIndex)
                Array.Copy(_buffer, _readIndex, array, offset, length);
            else
            {
                var firstPortion = Capacity - _readIndex;
                if (firstPortion > length)
                    firstPortion = length;
                Array.Copy(_buffer, _readIndex, array, offset, firstPortion);
                if (length - firstPortion > 0)
                    Array.Copy(_buffer, 0, array, offset + firstPortion, length - firstPortion);
            }
        }

        int _CopyToUnlocked(Memory<T> memory)
        {
            if (_CountNotLocked == 0 || memory.Length == 0)
                return 0;

            var length = memory.Length > _CountNotLocked ? _CountNotLocked : memory.Length;

            if (_readIndex < _writeIndex)
            {
                if (_writeIndex - _readIndex < length)
                    length = _writeIndex - _readIndex;
                _memBuffer.Slice(_readIndex, length).CopyTo(memory);
                return length;
            }
            else
            {
                var firstPortion = Capacity - _readIndex;
                if (firstPortion > length)
                    firstPortion = length;
                _memBuffer.Slice(_readIndex, firstPortion).CopyTo(memory);
                if (length - firstPortion == 0)
                    return firstPortion;

                _memBuffer.Slice(0, length - firstPortion).CopyTo(memory.Slice(firstPortion));
                return length;
            }
        }
        #endregion Copy

        #region Count and FreeSpace
        public int Count
        {
            get
            {
                lock (_lockObj)
                    return _CountNotLocked;
            }
        }

        int _CountNotLocked =>
            _readIndex == _writeIndex
                ? (_isFull ? Capacity : 0)
                : (_readIndex > _writeIndex ? Capacity : 0) + _writeIndex - _readIndex;

        public int FreeSpace
        {
            get
            {
                lock (_lockObj)
                    return Capacity - _CountNotLocked;
            }
        }
        #endregion Count

        #region Indexer
        public T this[int index]
        {
            get
            {
                lock(_lockObj)
                {
                    if (index < 0)
                        throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} must not be negative.");
                    if (index > _CountNotLocked)
                        throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot exceed {nameof(Count)}.");

                    return _AtIndexNotLocked(index);
                }
            }
        }

        T _AtIndexNotLocked(int index)
        {
            var newIndex = _readIndex + index;
            return newIndex >= Capacity
                ? _buffer[newIndex - Capacity]
                : _buffer[newIndex];
        }
        #endregion Indexer

        #region ToArray
        public T[] ToArray()
        {
            lock (_lockObj)
            {
                var output = new T[_CountNotLocked];
                if (_CountNotLocked > 0)
                    _CopyToNotLocked(output, 0, _CountNotLocked);
                return output;
            }
        }
        #endregion ToArray

        #region Read
        public int Read(T[] data, int offset, int length)
        {
            lock (_lockObj)
                return _ReadNotLocked(data, offset, length);
        }

        public int Read(Memory<T> memory)
        {
            lock (_lockObj)
                return _ReadNotLocked(memory);
        }

        int _ReadNotLocked(T[] data, int offset, int length)
        {
            var len = length > _CountNotLocked ? _CountNotLocked : length;
            _CopyToNotLocked(data, offset, len);
            if (len == _CountNotLocked)
                _ClearNotLocked();
            else
            {
                _readIndex += len;
                if (_readIndex >= Capacity)
                    _readIndex -= Capacity;
                _isFull = false;
            }
            return len;
        }

        int _ReadNotLocked(Memory<T> buffer)
        {
            var len = _CopyToUnlocked(buffer);
            if (len == _CountNotLocked)
                _ClearNotLocked();
            else
            {
                _readIndex += len;
                if (_readIndex >= Capacity)
                    _readIndex -= Capacity;
                _isFull = false;
            }
            return len;
        }
        #endregion Read

        #region Write
        public void Write(T[] data, int offset, int length)
        {
            if (length == 0)
                return;
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} must be larger or equal to zero.");
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} must be larger or equal to zero.");
            if (data.Length < length)
                throw new ArgumentException($"{nameof(data)} is smaller than {nameof(length)}.");
            if (data.Length - offset < length)
                throw new ArgumentException($"{nameof(data)} is smaller than {nameof(length)} with the provided {nameof(offset)}.");

            lock (_lockObj)
                _WriteNotLocked(data, offset, length);
        }

        public void Write(Memory<T> memory)
        {
            lock (_lockObj)
                _WriteNotLocked(memory);
        }

        void _WriteNotLocked(T[] data, int offset, int length)
        {
            if (length + _CountNotLocked > Capacity && !Overwrite)
                throw new ArgumentException($"Not enough space in the {nameof(ConcurrentCircularBuffer<T>)} for the {nameof(length)} given.");

            _isFull = length + _CountNotLocked >= Capacity;

            // We're way over capacity.
            // Just throw it all away and copy from the start
            if (length > Capacity)
            {
                Array.Copy(data, offset + length - Capacity, _buffer, 0, Capacity);
                _writeIndex = 0;
                _readIndex = 0;
                return;
            }

            if (_writeIndex + length >= Capacity)
            {
                var firstPortion = Capacity - _writeIndex;
                Array.Copy(data, offset, _buffer, _writeIndex, firstPortion);
                _writeIndex += firstPortion;
                if (_writeIndex == Capacity)
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

            if (_signal.CurrentCount == 0)
                _signal.Release(1);
        }

        void _WriteNotLocked(Memory<T> memory)
        {
            if (memory.Length + _CountNotLocked > Capacity && !Overwrite)
                throw new ArgumentException($"Not enough space in the {nameof(ConcurrentCircularBuffer<T>)} for the length of {nameof(memory)} provided.");

            _isFull = memory.Length + _CountNotLocked >= Capacity;

            // We're way over capacity.
            // Just throw it all away and copy from the start
            if (memory.Length > Capacity)
            {
                memory.Slice(memory.Length - Capacity).CopyTo(_memBuffer);
                _writeIndex = 0;
                _readIndex = 0;
                return;
            }

            var firstPortion = Capacity - _writeIndex;
            var length = memory.Length;
            if (_writeIndex + length >= Capacity)
            {
                memory.Slice(0, firstPortion).CopyTo(_memBuffer.Slice(_writeIndex));
                _writeIndex += firstPortion;
                if (_writeIndex == Capacity)
                    _writeIndex = 0;
                length -= firstPortion;
            }

            if (length > 0)
            {
                memory.Slice(firstPortion).CopyTo(_memBuffer);
                _writeIndex += length;
            }

            if (_isFull)
                _readIndex = _writeIndex;

            if (_signal.CurrentCount == 0)
                _signal.Release(1);
        }
        #endregion Write

        #region Blocking methods
        public T BlockedRemove() => BlockedRemove(CancellationToken.None);

        public T BlockedRemove(CancellationToken token)
        {
            while (true)
            {
                lock (_lockObj)
                {
                    if (_CountNotLocked > 0)
                        return _RemoveNotLocked();
                }
                _signal.Wait(token);
            }
        }

        public int BlockedRead(T[] data, int offset, int length) => BlockedRead(data, offset, length, CancellationToken.None);

        public int BlockedRead(T[] data, int offset, int length, CancellationToken token)
        {
            while (true)
            {
                lock (_lockObj)
                {
                    if (_CountNotLocked > 0)
                        return _ReadNotLocked(data, offset, length);
                }
                _signal.Wait(token);
            }
        }
        #endregion Blocking methods

        public int Capacity { get; } = DEFAULT_CAPACITY;
        public bool Overwrite { get; set; } = DEFAULT_OVERWRITE;
        public bool IsReadOnly => false;

        #region Internal index manipulation
        /// <summary>
        /// ++index
        /// </summary>
        int IncIndex(ref int index)
        {
            if (++index >= Capacity)
                index = 0;
            return index;
        }

        /// <summary>
        /// index++
        /// </summary>
        int IndexInc(ref int index)
        {
            var ind = index;
            if (++index >= Capacity)
                index = 0;
            return ind;
        }
        #endregion Internal index manipulation

        public static readonly int DEFAULT_CAPACITY = 1024;
        public static readonly bool DEFAULT_OVERWRITE = false;

        readonly SemaphoreSlim _signal;
        readonly object _lockObj = new object();
        readonly T[] _buffer;
        readonly Memory<T> _memBuffer;
        int _readIndex = 0;
        int _writeIndex = 0;
        bool _isFull = false;
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