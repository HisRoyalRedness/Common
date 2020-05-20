using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/*
    A circular buffer implementation for C#
    https://en.wikipedia.org/wiki/Circular_buffer

        The buffer can be initialised with OverWriting either enabled or
        disabled (disabled by default). If it is enabled, an exception
        will be thrown if the buffer capacity is exceeded, otherwise it
        will simply drop as many elements off the back as needed in order
        to stay within the capacity limit. A BufferOverflow event is 
        raised if items overflow the buffer.

        Data is added singularly with Add() and removed singularly with
        Remove(). The usual ICollection methods all work as expected.

        Read() and Write() have been provided to extract or add blocks
        of data respectively.

        All methods that effect (or depend on) internal state are run
        under an internal lock, so these operations should be atomic.

        BlockedRemove and BlockedRead methods will block until there are
        items in the buffer that can be removed. This is usefull for 
        Producser / Consumer patterns.


    Keith Fletcher
    May 2020

    https://github.com/HisRoyalRedness/Common/blob/master/C%23/ConcurrentCircularBuffer.cs

    This file is Unlicensed.
    See the foot of the file, or refer to <http://unlicense.org>
*/

namespace HisRoyalRedness.com
{
    /// <summary>
    /// A circular buffer implementation with some internal thread locking
    /// to make operations atomic.
    /// </summary>
    /// <typeparam name="T">The type of element held by the buffer.</typeparam>
    public class ConcurrentCircularBuffer<T> : ICollection<T>, IProducerConsumerCollection<T>
    {
        #region Construction
        /// <summary>
        /// Create a new instance of <see cref="ConcurrentCircularBuffer{T}"/>,
        /// with a default <see cref="Capacity"/> of <see cref="DEFAULT_CAPACITY"/>.
        /// <see cref="Overwrite"/> defaults to false.
        /// </summary>
        public ConcurrentCircularBuffer()
            : this(Enumerable.Empty<T>(), DEFAULT_CAPACITY, DEFAULT_OVERWRITE)
        { }

        /// <summary>
        /// Create a new instance of <see cref="ConcurrentCircularBuffer{T}"/>,
        /// with the specified <paramref name="capacity"/>.
        /// <see cref="Overwrite"/> defaults to false.
        /// </summary>
        /// <param name="capacity">The maximum number of elements that the
        /// <see cref="CircularBuffer{T}"/> can hold</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is not larger than 0.</exception>
        public ConcurrentCircularBuffer(int capacity)
            : this(Enumerable.Empty<T>(), capacity, DEFAULT_OVERWRITE)
        { }

        /// <summary>
        /// Create a new instance of <see cref="ConcurrentCircularBuffer{T}"/>,
        /// with a default <see cref="Capacity"/> of <see cref="DEFAULT_CAPACITY"/>.
        /// </summary>
        /// <param name="overwrite">If <paramref name="overwrite"/> is false, an <see cref="InvalidOperationException"/>
        /// is thrown if you exceed the buffer size. When <paramref name="overwrite"/>
        /// is true, newer data overwrites the older elements in the buffer.</param>
        public ConcurrentCircularBuffer(bool overwrite)
            : this(Enumerable.Empty<T>(), DEFAULT_CAPACITY, overwrite)
        { }

        /// <summary>
        /// Create a new instance of <see cref="ConcurrentCircularBuffer{T}"/>,
        /// with the specified <paramref name="capacity"/>.
        /// </summary>
        /// <param name="capacity">The maximum number of elements that the
        /// <see cref="ConcurrentCircularBuffer{T}"/> can hold</param>
        /// <param name="overwrite">If <paramref name="overwrite"/> is false, an <see cref="InvalidOperationException"/>
        /// is thrown if you exceed the buffer size. When <paramref name="overwrite"/>
        /// is true, newer data overwrites the older elements in the buffer.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is not larger than 0.</exception>
        public ConcurrentCircularBuffer(int capacity, bool overwrite)
            : this(Enumerable.Empty<T>(), capacity, overwrite)
        { }

        /// <summary>
        /// Create a new instance of <see cref="ConcurrentCircularBuffer{T}"/>,
        /// with a default <see cref="Capacity"/> of <see cref="DEFAULT_CAPACITY"/>.
        /// <see cref="Overwrite"/> defaults to false.</summary>
        /// <param name="initialItems">A collection of itmes to add as the initial
        /// items in the buffer.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is not larger than 0.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="initialItems"/> is null.</exception>
        public ConcurrentCircularBuffer(IEnumerable<T> initialItems)
            : this(initialItems, DEFAULT_CAPACITY, DEFAULT_OVERWRITE)
        { }

        /// <summary>
        /// Create a new instance of <see cref="ConcurrentCircularBuffer{T}"/>,
        /// with the specified <paramref name="capacity"/>.
        /// <see cref="Overwrite"/> defaults to false.
        /// </summary>
        /// <param name="initialItems">A collection of itmes to add as the initial
        /// items in the buffer.</param>
        /// <param name="capacity">The maximum number of elements that the
        /// <see cref="CircularBuffer{T}"/> can hold</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is not larger than 0.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="initialItems"/> is null.</exception>
        public ConcurrentCircularBuffer(IEnumerable<T> initialItems, int capacity)
            : this(initialItems, capacity, DEFAULT_OVERWRITE)
        { }

        /// <summary>
        /// Create a new instance of <see cref="ConcurrentCircularBuffer{T}"/>,
        /// with a default <see cref="Capacity"/> of <see cref="DEFAULT_CAPACITY"/>.
        /// </summary>
        /// <param name="initialItems">A collection of itmes to add as the initial
        /// items in the buffer.</param>
        /// <param name="overwrite">If <paramref name="overwrite"/> is false, an <see cref="InvalidOperationException"/>
        /// is thrown if you exceed the buffer size. When <paramref name="overwrite"/>
        /// is true, newer data overwrites the older elements in the buffer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="initialItems"/> is null.</exception>
        public ConcurrentCircularBuffer(IEnumerable<T> initialItems, bool overwrite)
            : this(initialItems, DEFAULT_CAPACITY, overwrite)
        { }

        /// <summary>
        /// Create a new instance of <see cref="ConcurrentCircularBuffer{T}"/>,
        /// with the specified <paramref name="capacity"/>.
        /// </summary>
        /// <param name="initialItems">A collection of itmes to add as the initial
        /// items in the buffer.</param>
        /// <param name="capacity">The maximum number of elements that the
        /// <see cref="ConcurrentCircularBuffer{T}"/> can hold</param>
        /// <param name="overwrite">If <paramref name="overwrite"/> is false, an <see cref="InvalidOperationException"/>
        /// is thrown if you exceed the buffer size. When <paramref name="overwrite"/>
        /// is true, newer data overwrites the older elements in the buffer.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is not larger than 0.</exception>
        /// /// <exception cref="ArgumentNullException"><paramref name="initialItems"/> is null.</exception>
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
        /// <summary>
        /// Returns an Enumerator that iterates over the elements in the 
        /// <see cref="ConcurrentCircularBuffer{T}"/>.
        /// The elements are not removed. Altering the buffer (e.g. Add, Remove
        /// or Clear) will invalidate the enumerator.
        /// NOT THREAD SAFE!
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="ConcurrentCircularBuffer{T}"/>.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; ++i)
                yield return this[i];
        }

        /// <summary>
        /// Returns an Enumerator that iterates over the elements in the 
        /// <see cref="ConcurrentCircularBuffer{T}"/>.
        /// The elements are not removed. Altering the buffer (e.g. Add, Remove
        /// or Clear) will invalidate the enumerator.
        /// NOT THREAD SAFE!
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> for the <see cref="ConcurrentCircularBuffer{T}"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion IEnumerable<T> and IEnumerable implementation

        #region Add
        /// <summary>
        /// Add an <paramref name="item"/> to the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// If the buffer is full and <see cref="Overwrite"/> is false, a
        /// <see cref="InvalidOperationException"/> is thrown. If 
        /// <see cref="Overwrite"/> is true, a <see cref="BufferOverflow"/> event
        /// is raised. True is always returned.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <returns>Always true</returns>
        /// <exception cref="InvalidOperationException">The buffer is already full
        /// and <see cref="Overwrite"/> is false.</exception>
        public bool TryAdd(T item)
        {
            Add(item);
            return true;
        }

        /// <summary>
        /// Add an <paramref name="item"/> to the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// If the buffer is full and <see cref="Overwrite"/> is false, a
        /// <see cref="InvalidOperationException"/> is thrown. If 
        /// <see cref="Overwrite"/> is true, a <see cref="BufferOverflow"/> event
        /// is raised.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <exception cref="InvalidOperationException">The buffer is already full
        /// and <see cref="Overwrite"/> is false.</exception>
        public void Add(T item) => Add(item, Overwrite);

        /// <summary>
        /// Add an <paramref name="item"/> to the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// If the buffer is full and <see cref="Overwrite"/> is false, a
        /// <see cref="InvalidOperationException"/> is thrown. If 
        /// <see cref="Overwrite"/> is true, a <see cref="BufferOverflow"/> event
        /// is raised.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <param name="overwrite">True to allow old items to be overwritten if
        /// the buffer is full, false otherwise.</param>
        /// <exception cref="InvalidOperationException">The buffer is already full
        /// and <see cref="Overwrite"/> is false.</exception>
        public void Add(T item, bool overwrite)
        {
            lock (_lockObj)
            { 
                _AddNotLocked(item, overwrite);
            } 
        }

        void _AddNotLocked(T item, bool overwrite)
        {
            if (_isFull)
            {
                if (Overwrite)
                    OnBufferOverflow(1);
                else
                    throw new InvalidOperationException($"Cannot {nameof(Add)} another {nameof(item)}. The {nameof(ConcurrentCircularBuffer<T>)} is full.");
            }

            _buffer[IndexInc(ref _writeIndex)] = item;
            if (_isFull)
                _readIndex = _writeIndex;
            else
                _isFull = (_writeIndex == _readIndex);

            if (_signal.CurrentCount == 0)
                _signal.Release(1);
        }
        #endregion Add

        #region Remove / Take
        /// <summary>
        /// Not supported
        /// </summary>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public bool Remove(T item) => throw new NotSupportedException("Removing by item is not supported");

        /// <summary>
        /// Remove a single <typeparamref name="T"/> instance from the buffer and return it.
        /// </summary>
        /// <returns>The last <typeparamref name="T"/> item in the buffer </returns>
        /// <exception cref="InvalidOperationException">The <see cref="ConcurrentCircularBuffer{T}"/> is empty.</exception>
        public T Remove()
        {
            lock (_lockObj)
            {
                return _RemoveNotLocked();
            }
        }

        /// <summary>
        /// Remove a number of items from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="length">The number of items to remove.
        /// Fewer then <paramref name="length"/> items may be removed if there aren't enough items in the
        /// <see cref="ConcurrentCircularBuffer{T}"/>.</param>
        /// <returns>The number of items removed from <see cref="ConcurrentCircularBuffer{T}"/>.</returns>
        public int Remove(int length)
        {
            lock (_lockObj)
            {
                return _RemoveNotLocked(length);
            }
        }

        /// <summary>
        /// Tries to remove an item from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="item">The item to be removed from the collection.</param>
        /// <returns>true if an item could be removed; otherwise, false.</returns>
        public bool TryTake(out T item)
        {
            lock (_lockObj)
            {
                if (_CountNotLocked == 0)
                {
                    item = default;
                    return false;
                }
                else
                {
                    item = _RemoveNotLocked();
                    return true;
                }
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
        #endregion Remove / Take

        #region Clear
        /// <summary>
        /// Removes all elements from the buffer.
        /// </summary>
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
        /// <summary>
        /// Looks for an item in the <see cref="ConcurrentCircularBuffer{T}"/>
        /// using item.Equals().
        /// <param name="item">The item to search for.</param>
        /// <returns>Returns true if an item was found that equals <paramref name="item"/>.
        /// Returns false otherwise, or if <paramref name="item"/> is null.</returns>
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
        /// <summary>
        /// Copy data from the <see cref="ConcurrentCircularBuffer{T}"/> to an array.
        /// The items are not removed from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Use <see cref="Read(T[], int, int)"/> if you want to copy items
        /// and remove them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="array">The array to copy into.</param>
        /// <param name="offset">The offset in <paramref name="array"/> at which to begin copying.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
        /// <exception cref="InvalidOperationException">There is not enough space in <paramref name="array"/> 
        /// (starting from <paramref name="offset"/>) to hold all the data.</exception>
        public void CopyTo(T[] array, int offset) => CopyTo(array, offset, Count);

        /// <summary>
        /// Copy data from the <see cref="ConcurrentCircularBuffer{T}"/> to an array.
        /// The items are not removed from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Use <see cref="Read(T[], int, int)"/> if you want to copy items
        /// and remove them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="array">The array to copy into.</param>
        /// <param name="offset">The offset in <paramref name="array"/> at which to begin copying.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
        /// <exception cref="InvalidOperationException">There is not enough space in <paramref name="array"/> 
        /// (starting from <paramref name="offset"/>) to hold all the data.</exception>
        public void CopyTo(Array array, int offset) => CopyTo((T[])array, offset, Count);

        /// <summary>
        /// Copy data from the <see cref="ConcurrentCircularBuffer{T}"/> to an array.
        /// The items are not removed from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Use <see cref="Read(T[], int, int)"/> if you want to copy items
        /// and remove them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="array">The array to copy into.</param>
        /// <param name="offset">The offset in <paramref name="array"/> at which to begin copying.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
        /// <exception cref="InvalidOperationException">There is not enough space in <paramref name="array"/> 
        /// (starting from <paramref name="offset"/>) to hold all the data.</exception>
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

        /// <summary>
        /// Copy data from the <see cref="ConcurrentCircularBuffer{T}"/> to an <see cref="Memory{T}"/> .
        /// The items are not removed from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Use <see cref="Read(Memory{T})"/> if you want to copy items
        /// and remove them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="memory">The memory to copy into.</param>
        /// <returns>The number of items copied into the <see cref="Memory{T}"/> </returns>
        /// <exception cref="ArgumentNullException"><paramref name="memory"/> is null.</exception>
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
        /// <summary>
        /// Returns the actual number of elements in the <see cref="ConcurrentCircularBuffer{T}"/>
        /// </summary>
        /// <returns>The actual number of elements in the <see cref="ConcurrentCircularBuffer{T}"/></returns>
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

        /// <summary>
        /// Returns the actual number of items that can still be stored in the 
        /// <see cref="ConcurrentCircularBuffer{T}"/> before running out of space.
        /// </summary>
        /// <returns>The number of empty slots in the <see cref="ConcurrentCircularBuffer{T}"/></returns>
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
        /// <summary>
        /// Return a single item from the <see cref="ConcurrentCircularBuffer{T}"/>
        /// without removing it. 
        /// </summary>
        /// <param name="index">The zero-based index of the item.</param>
        /// <returns>The item at the zero-based index in the <see cref="CircularBuffer{T}"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> exceeds <see cref="Count"/>.</exception>
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
        /// <summary>
        /// Creates an array from a <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Items are not removed from the buffer.
        /// </summary>
        /// <returns>An array that contains the elements of the <see cref="ConcurrentCircularBuffer{T}"/> instance.</returns>
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
        /// <summary>
        /// Remove items from the <see cref="ConcurrentCircularBuffer{T}"/>, and copies
        /// them into <paramref name="data"/>.
        /// Use <see cref="CopyTo(T[], int, int)"/> if you want to copy items
        /// without removing them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Fewer then <paramref name="length"/> items may be copied if there aren't enough items in the
        /// <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="data">The array to copy the items into.</param>
        /// <param name="offset">The offset in <paramref name="data"/> at which to begin copying.</param>
        /// <param name="length">The number of items to copy from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Fewer then <paramref name="length"/> items may be copied if there aren't enough items in the
        /// <see cref="ConcurrentCircularBuffer{T}"/>.</param>
        /// <returns>The number of items copied to <paramref name="data"/> and removed from <see cref="ConcurrentCircularBuffer{T}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
        /// <exception cref="InvalidOperationException">There is not enough space in <paramref name="data"/> 
        /// (starting from <paramref name="offset"/>) to hold all the data.</exception>
        public int Read(T[] data, int offset, int length)
        {
            lock (_lockObj)
                return _ReadNotLocked(data, offset, length);
        }

        /// <summary>
        /// Remove items from the <see cref="ConcurrentCircularBuffer{T}"/>, and copies
        /// them into <paramref name="memory"/>.
        /// Use <see cref="CopyTo(Memory{T})"/> if you want to copy items
        /// without removing them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Fewer items than the length of <paramref name="memory"/> may be copied if there aren't enough items in the
        /// <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="memory">The <see cref="Memory{T}"/> to copy the items into.</param>
        /// <returns>The number of items copied to <paramref name="memory"/> and removed from <see cref="ConcurrentCircularBuffer{T}"/>.</returns>
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
        /// <summary>
        /// Extract <paramref name="length"/> items from <paramref name="data"/>,
        /// starting at <paramref name="offset"/>, then add them to the
        /// <see cref="ConcurrentCircularBuffer{T}"/> instance.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"><paramref name="data"/> 
        /// is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref 
        /// name="offset"/> is negative.</exception>
        /// <exception cref="System.ArgumentException"><paramref name="data"/> is 
        /// smaller than <paramref name="length"/> considering the <paramref 
        /// name="offset"/> given.</exception>
        /// <exception cref="System.ArgumentException">There is not enough space to 
        /// for <paramref name="length"/> items and overwriting is not permitted.</exception>
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

        /// <summary>
        /// Copy items from <paramref name="memory"/> into the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <exception cref="System.ArgumentException">There is not enough space to 
        /// for <paramref name="length"/> items and overwriting is not permitted.</exception>
        public void Write(ReadOnlyMemory<T> memory)
        {
            lock (_lockObj)
                _WriteNotLocked(memory);
        }

        void _WriteNotLocked(T[] data, int offset, int length)
        {
            if (length + _CountNotLocked > Capacity)
            {
                if (Overwrite)
                    OnBufferOverflow(Capacity - (length + _CountNotLocked));
                else
                    throw new ArgumentException($"Not enough space in the {nameof(ConcurrentCircularBuffer<T>)} for the {nameof(length)} given.");
            }

            // We're equal to or over capacity.
            // Just throw it all away and copy from the start
            if (length >= Capacity)
            {
                Array.Copy(data, offset + length - Capacity, _buffer, 0, Capacity);
                _isFull = true;
                _writeIndex = 0;
                _readIndex = 0;
                return;
            }

            _isFull = length + _CountNotLocked >= Capacity;

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

        void _WriteNotLocked(ReadOnlyMemory<T> memory)
        {
            if (memory.Length + _CountNotLocked > Capacity)
            {
                if (Overwrite)
                    OnBufferOverflow(Capacity - (memory.Length + _CountNotLocked));
                else
                    throw new ArgumentException($"Not enough space in the {nameof(ConcurrentCircularBuffer<T>)} for the length of {nameof(memory)} given.");
            }

            // We're equal to or over capacity.
            // Just throw it all away and copy from the start
            if (memory.Length >= Capacity)
            {
                memory.Slice(memory.Length - Capacity).CopyTo(_memBuffer);
                _isFull = true;
                _writeIndex = 0;
                _readIndex = 0;
                return;
            }

            _isFull = memory.Length + _CountNotLocked >= Capacity;

            var firstPortion = Capacity - _writeIndex;
            var length = memory.Length;
            var offset = 0;
            if (_writeIndex + length >= Capacity)
            {
                memory.Slice(0, firstPortion).CopyTo(_memBuffer.Slice(_writeIndex));
                _writeIndex += firstPortion;
                if (_writeIndex == Capacity)
                    _writeIndex = 0;
                length -= firstPortion;
                offset = firstPortion;
            }

            if (length > 0)
            {
                memory.Slice(offset).CopyTo(_memBuffer);
                _writeIndex += length;
            }

            if (_isFull)
                _readIndex = _writeIndex;

            if (_signal.CurrentCount == 0)
                _signal.Release(1);
        }
        #endregion Write

        #region Blocking methods
        /// <summary>
        /// Attempt to remove an item from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// The thread blocks if there are no items to remove, until at least one item
        /// is added.
        /// </summary>
        /// <returns>The removed item</returns>
        public T BlockedRemove() => BlockedRemove(CancellationToken.None);

        /// <summary>
        /// Attempt to remove an item from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// The thread blocks if there are no items to remove, until at least one item
        /// is added.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> to cancel the blocking operation.</param>
        /// <returns>The removed item</returns>
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

        /// <summary>
        /// Remove items from the <see cref="ConcurrentCircularBuffer{T}"/>, and copies
        /// them into <paramref name="data"/>.
        /// If there are no items to remove, the thread blocks until at least one item is added.
        /// Use <see cref="CopyTo(T[], int, int)"/> if you want to copy items
        /// without removing them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Fewer then <paramref name="length"/> items may be copied if there aren't enough items in the
        /// <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="data">The array to copy the items into.</param>
        /// <param name="offset">The offset in <paramref name="data"/> at which to begin copying.</param>
        /// <param name="length">The number of items to copy from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Fewer then <paramref name="length"/> items may be copied if there aren't enough items in the
        /// <see cref="ConcurrentCircularBuffer{T}"/>.</param>
        /// <returns>The number of items copied to <paramref name="data"/> and removed from <see cref="ConcurrentCircularBuffer{T}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
        /// <exception cref="InvalidOperationException">There is not enough space in <paramref name="data"/> 
        /// (starting from <paramref name="offset"/>) to hold all the data.</exception>
        public int BlockedRead(T[] data, int offset, int length) => BlockedRead(data, offset, length, CancellationToken.None);

        /// <summary>
        /// Remove items from the <see cref="ConcurrentCircularBuffer{T}"/>, and copies
        /// them into <paramref name="data"/>.
        /// If there are no items to remove, the thread blocks until at least one item is added.
        /// Use <see cref="CopyTo(T[], int, int)"/> if you want to copy items
        /// without removing them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Fewer then <paramref name="length"/> items may be copied if there aren't enough items in the
        /// <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="data">The array to copy the items into.</param>
        /// <param name="offset">The offset in <paramref name="data"/> at which to begin copying.</param>
        /// <param name="length">The number of items to copy from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Fewer then <paramref name="length"/> items may be copied if there aren't enough items in the
        /// <see cref="ConcurrentCircularBuffer{T}"/>.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to cancel the blocking operation.</param>
        /// <returns>The number of items copied to <paramref name="data"/> and removed from <see cref="ConcurrentCircularBuffer{T}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
        /// <exception cref="InvalidOperationException">There is not enough space in <paramref name="data"/> 
        /// (starting from <paramref name="offset"/>) to hold all the data.</exception>
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

        /// <summary>
        /// Remove items from the <see cref="ConcurrentCircularBuffer{T}"/>, and copies
        /// them into <paramref name="data"/>.
        /// If there are no items to remove, the thread blocks until at least one item is added.
        /// Use <see cref="CopyTo(T[], int, int)"/> if you want to copy items
        /// without removing them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Fewer then <paramref name="length"/> items may be copied if there aren't enough items in the
        /// <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="data">The array to copy the items into.</param>
        /// <param name="offset">The offset in <paramref name="data"/> at which to begin copying.</param>
        /// <param name="length">The number of items to copy from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Fewer then <paramref name="length"/> items may be copied if there aren't enough items in the
        /// <see cref="ConcurrentCircularBuffer{T}"/>.</param>
        /// <returns>The number of items copied to <paramref name="data"/> and removed from <see cref="ConcurrentCircularBuffer{T}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
        /// <exception cref="InvalidOperationException">There is not enough space in <paramref name="data"/> 
        /// (starting from <paramref name="offset"/>) to hold all the data.</exception>
        public Task<int> BlockedReadAsync(T[] data, int offset, int length) => BlockedReadAsync(data, offset, length, CancellationToken.None);

        /// <summary>
        /// Remove items from the <see cref="ConcurrentCircularBuffer{T}"/>, and copies
        /// them into <paramref name="data"/>.
        /// If there are no items to remove, the thread blocks until at least one item is added.
        /// Use <see cref="CopyTo(T[], int, int)"/> if you want to copy items
        /// without removing them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Fewer then <paramref name="length"/> items may be copied if there aren't enough items in the
        /// <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="data">The array to copy the items into.</param>
        /// <param name="offset">The offset in <paramref name="data"/> at which to begin copying.</param>
        /// <param name="length">The number of items to copy from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// Fewer then <paramref name="length"/> items may be copied if there aren't enough items in the
        /// <see cref="ConcurrentCircularBuffer{T}"/>.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to cancel the blocking operation.</param>
        /// <returns>The number of items copied to <paramref name="data"/> and removed from <see cref="ConcurrentCircularBuffer{T}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
        /// <exception cref="InvalidOperationException">There is not enough space in <paramref name="data"/> 
        /// (starting from <paramref name="offset"/>) to hold all the data.</exception>
        public async Task<int> BlockedReadAsync(T[] data, int offset, int length, CancellationToken token)
        {
            while (true)
            {
                lock (_lockObj)
                {
                    if (_CountNotLocked > 0)
                        return _ReadNotLocked(data, offset, length);
                }
                await _signal.WaitAsync(token);
            }
        }

        /// <summary>
        /// Remove items from the <see cref="ConcurrentCircularBuffer{T}"/>, and copies
        /// them into <paramref name="memory"/>.
        /// If there are no items to remove, the thread blocks until at least one item is added.
        /// Use <see cref="CopyTo(Memory{T})"/> if you want to copy items
        /// without removing them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="memory">The <see cref="Memory{T}"/> to copy the items into.</param>
        /// <returns>The number of items copied to <paramref name="data"/> and removed from <see cref="ConcurrentCircularBuffer{T}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <returns>The number of items read from the buffer.</returns>
        public int BlockedRead(Memory<T> memory) => BlockedRead(memory, CancellationToken.None);

        /// <summary>
        /// Remove items from the <see cref="ConcurrentCircularBuffer{T}"/>, and copies
        /// them into <paramref name="memory"/>.
        /// If there are no items to remove, the thread blocks until at least one item is added.
        /// Use <see cref="CopyTo(Memory{T})"/> if you want to copy items
        /// without removing them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="memory">The <see cref="Memory{T}"/> to copy the items into.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to cancel the blocking operation.</param>
        /// <returns>The number of items copied to <paramref name="data"/> and removed from <see cref="ConcurrentCircularBuffer{T}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <returns>The number of items read from the buffer.</returns>
        public int BlockedRead(Memory<T> memory, CancellationToken token)
        {
            while (true)
            {
                lock (_lockObj)
                {
                    if (_CountNotLocked > 0)
                        return _ReadNotLocked(memory);
                }
                _signal.Wait(token);
            }
        }

        /// <summary>
        /// Remove items from the <see cref="ConcurrentCircularBuffer{T}"/>, and copies
        /// them into <paramref name="memory"/>.
        /// If there are no items to remove, the thread blocks until at least one item is added.
        /// Use <see cref="CopyTo(Memory{T})"/> if you want to copy items
        /// without removing them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="memory">The <see cref="Memory{T}"/> to copy the items into.</param>
        /// <returns>The number of items copied to <paramref name="data"/> and removed from <see cref="ConcurrentCircularBuffer{T}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <returns>The number of items read from the buffer.</returns>
        public Task<int> BlockedReadAsync(Memory<T> memory) => BlockedReadAsync(memory, CancellationToken.None);

        /// <summary>
        /// Remove items from the <see cref="ConcurrentCircularBuffer{T}"/>, and copies
        /// them into <paramref name="memory"/>.
        /// If there are no items to remove, the thread blocks until at least one item is added.
        /// Use <see cref="CopyTo(Memory{T})"/> if you want to copy items
        /// without removing them from the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// </summary>
        /// <param name="memory">The <see cref="Memory{T}"/> to copy the items into.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to cancel the blocking operation.</param>
        /// <returns>The number of items copied to <paramref name="data"/> and removed from <see cref="ConcurrentCircularBuffer{T}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <returns>The number of items read from the buffer.</returns>
        public async Task<int> BlockedReadAsync(Memory<T> memory, CancellationToken token)
        {
            while (true)
            {
                lock (_lockObj)
                {
                    if (_CountNotLocked > 0)
                        return _ReadNotLocked(memory);
                }
                await _signal.WaitAsync(token);
            }
        }
        #endregion Blocking methods

        #region Events
        public delegate void BufferOverflowEventHandler(object sender, EventArgs args);

        /// <summary>
        /// The <see cref="BufferOverflow"/> event is raised if items are added to the 
        /// <see cref="ConcurrentCircularBuffer{T}"/> and the <see cref="Capacity"/>
        /// is exceeded, and <see cref="Overwrite"/> is set to true.
        /// </summary>
        public event BufferOverflowEventHandler BufferOverflow;

        public class BufferOverflowEventArgs : EventArgs
        {
            public BufferOverflowEventArgs(int overflowCount, string message = null)
            {
                OverflowCount = overflowCount;
                Message = string.IsNullOrWhiteSpace(message)
                    ? $"The {nameof(ConcurrentCircularBuffer<T>)} overflowed by {overflowCount} {(overflowCount == 1 ? "item" : "items")}"
                    : message;
            }

            public int OverflowCount { get; }
            public string Message { get; }
        }
        protected void OnBufferOverflow(int overflowCount, string message = null) => BufferOverflow?.Invoke(this, new BufferOverflowEventArgs(overflowCount, message));

        #endregion Events

        /// <summary>
        /// The maximum number of items the <see cref="ConcurrentCircularBuffer{T}"/> can hold.
        /// </summary>
        public int Capacity { get; } = DEFAULT_CAPACITY;
        /// <summary>
        /// If true, an item added to a full buffer will cause
        /// an item to be dropped from the end of the <see cref="ConcurrentCircularBuffer{T}"/>.
        /// If false, and item added to a full <see cref="ConcurrentCircularBuffer{T}"/>
        /// will result in an <see cref="InvalidOperationException"/>.
        /// </summary>
        public bool Overwrite { get; set; } = DEFAULT_OVERWRITE;
        /// <summary>
        /// Always returns false. The <see cref="ConcurrentCircularBuffer{T}"/>
        /// cannot be made read-only.
        /// </summary>
        /// <returns>False</returns>
        public bool IsReadOnly => false;
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => _lockObj;

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