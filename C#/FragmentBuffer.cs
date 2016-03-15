using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    A read-only wrapper around an array, that allows us to create fragments of the original
    without having to make copies. All derived fragments still point to the one original.

        File description (long)

    Keith Fletcher
    Mar 2016

    This file is Unlicensed.
    See the foot of the file, or refer to <http://unlicense.org>
*/

namespace fletcher.org
{
    /// <summary>
    /// A read-only wrapper around an array, that allows us to create fragments of the original
    /// without having to make copies. All derived fragments still point to the one original.
    /// </summary>
    /// <typeparam name="T">The type of element held by the buffer.</typeparam>
    public class FragmentBuffer<T> : IReadOnlyList<T>
    {
        #region Constructors
        /// <summary>
        /// Create an empty buffer
        /// </summary>
        public FragmentBuffer()
        {
            _buffer = new T[0];
            _offset = 0;
            _count = 0;
        }

        /// <summary>
        /// Create a buffer around <paramref name="baseBuffer"/>.
        /// Buffer[0] points to <paramref name="baseBuffer"/>[<paramref name="offset"/>].
        /// </summary>
        public FragmentBuffer(T[] baseBuffer, int offset, int count)
        {
            if (baseBuffer == null)
                throw new ArgumentNullException(nameof(baseBuffer));
            if (offset < 0 || offset > baseBuffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0 || count > baseBuffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (count + offset > baseBuffer.Length)
                throw new ArgumentOutOfRangeException($"The sum of {nameof(offset)} and {nameof(count)} exceed the size of {nameof(baseBuffer)}.");

            _buffer = baseBuffer;
            _offset = offset;
            _count = count;
        }

        /// <summary>
        /// Wrap a fragment of another <see cref="FragmentBuffer{T}"/>. The original
        /// array is not copied.
        /// </summary>
        public FragmentBuffer(FragmentBuffer<T> buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0 || offset > buffer.Count)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0 || count > buffer.Count)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (count + offset > buffer.Count)
                throw new ArgumentOutOfRangeException($"The sum of {nameof(offset)} and {nameof(count)} exceed the size of {nameof(buffer)}.");

            _buffer = buffer._buffer;
            _offset = buffer._offset + offset;
            _count = count;
            CreateCachedString();
        }
        #endregion Constructors

        public static FragmentBuffer<T> CreateCopy(T[] baseBuffer, int offset, int count) => new FragmentBuffer<T>(baseBuffer, offset, count, true);
        public static FragmentBuffer<T> CreateCopy(IEnumerable<T> buffer) => new FragmentBuffer<T>(buffer);

        public T[] ToArray()
        {
            var output = new T[Count];
            CopyTo(output, 0);
            return output;
        }

        public int Offset => _offset;

        #region IReadOnlyList implementation
        public T this[int index] => _buffer[index + _offset];

        public int Count => _count;
        #endregion IReadOnlyList implementation

        #region IEnumerable implementation
        [DebuggerStepThrough]
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _count; ++i)
                yield return this[i];
        }

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator()
        {
            for (var i = 0; i < _count; ++i)
                yield return this[i];
        }
        #endregion IEnumerable implementation

        #region IList Implementation
        public bool IsReadOnly => true;

        [DebuggerStepThrough]
        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; ++i)
                if (this[i].Equals(item))
                    return i;
            return -1;
        }

        [DebuggerStepThrough]
        public bool Contains(T item) => IndexOf(item) != -1;

        [DebuggerStepThrough]
        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < Count; ++i)
                array[arrayIndex + i] = this[i];
        }

        T IList<T>.this[int index]
        {
            get { return _buffer[index + _offset]; }
            set { throw new NotSupportedException(); }
        }

        [DebuggerStepThrough]
        public void Insert(int index, T item)
        { throw new NotSupportedException(); }

        [DebuggerStepThrough]
        public void RemoveAt(int index)
        { throw new NotSupportedException(); }

        [DebuggerStepThrough]
        public void Add(T item)
        { throw new NotSupportedException(); }

        [DebuggerStepThrough]
        public void Clear()
        { throw new NotSupportedException(); }

        [DebuggerStepThrough]
        public bool Remove(T item)
        { throw new NotSupportedException(); }
        #endregion IList Implementation

        public override string ToString() => _cachedString;

        void CreateCachedString()
        {
            //if (typeof(T) == typeof(byte))
            //    _cachedString = $"[{Count}]: {(this as Buffer<byte>).ToHexString()}";
            //else
            //    _cachedString = _buffer.ToString();
        }

        readonly protected T[] _buffer;
        readonly protected int _offset;
        readonly protected int _count;

        string _cachedString = null;
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
