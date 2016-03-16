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
        [DebuggerStepThrough]
        public FragmentBuffer()
        {
            _buffer = new T[0];
            _offset = 0;
            _count = 0;
        }

        /// <summary>
        /// Create a wrapper around <paramref name="baseBuffer"/>.
        /// Buffer[0] points to <paramref name="baseBuffer"/>[<paramref name="offset"/>].
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="baseBuffer" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><param name="offset" /> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><param name="length" /> is negative.</exception>
        /// <exception cref="InvalidOperationException">The sum of <param name="length" /> and <param name="offset" /> 
        /// exceed the size of <param name="baseBuffer" />.</exception>
        [DebuggerStepThrough]
        public FragmentBuffer(T[] baseBuffer, int offset, int length)
        {
            if (baseBuffer == null)
                throw new ArgumentNullException(nameof(baseBuffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} cannot be negative.");
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} cannot be negative.");
            if (length + offset > baseBuffer.Length)
                throw new InvalidOperationException($"The sum of {nameof(offset)} and {nameof(length)} exceed the size of {nameof(baseBuffer)}.");
            _buffer = baseBuffer;
            _offset = offset;
            _count = length;
        }

        /// <summary>
        /// Wrap a fragment of another <see cref="FragmentBuffer{T}"/>. The original
        /// array is not copied.
        /// Buffer[0] points to <paramref name="buffer"/>[<paramref name="offset"/>].
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="buffer" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><param name="offset" /> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><param name="length" /> is negative.</exception>
        /// <exception cref="InvalidOperationException">The sum of <param name="length" /> and <param name="offset" /> 
        /// exceed the size of <param name="buffer" />.</exception>
        [DebuggerStepThrough]
        public FragmentBuffer(FragmentBuffer<T> buffer, int offset, int length)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} cannot be negative." );
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} cannot be negative.");
            if (length + offset > buffer.Count)
                throw new InvalidOperationException($"The sum of {nameof(offset)} and {nameof(length)} exceed the size of {nameof(buffer)}.");

            _buffer = buffer._buffer;
            _offset = buffer._offset + offset;
            _count = length;
        }
        #endregion Constructors

        /// <summary>
        /// The offset into the original array when this instance was constructed.
        /// </summary>
        public int Offset => _offset;

        #region IReadOnlyList<T> implementation
        /// <summary>
        /// Returns the actual number of elements in the <see cref="FragmentBuffer{T}"/>
        /// </summary>
        /// <returns>The actual number of elements in the <see cref="FragmentBuffer{T}"/></returns>        
        public int Count => _count;

        /// <summary>
        /// Return a single item from the <see cref="FragmentBuffer{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the item.</param>
        /// <returns>The item at the zero-based index in the <see cref="FragmentBuffer{T}"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> exceeds <see cref="Count"/>.</exception>
        public T this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} must not be negative.");
                if (index > Count)
                    throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot exceed {nameof(Count)}.");

                return _buffer[index + _offset];
            }
        }

        /// <summary>
        /// Iterate over the elements in the <see cref="FragmentBuffer{T}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="FragmentBuffer{T}"/>.</returns>
        [DebuggerStepThrough]
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _count; ++i)
                yield return this[i];
        }

        /// <summary>
        /// Iterate over the elements in the <see cref="FragmentBuffer{T}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="FragmentBuffer{T}"/>.</returns>
        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion IReadOnlyList<T> implementation

        /// <summary>
        /// Copy data from the <see cref="FragmentBuffer{T}"/> to an array.
        /// </summary>
        /// <param name="data">The array to copy into.</param>
        /// <param name="offset">The offset in <paramref name="data"/> at which to begin copying.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
        /// <exception cref="InvalidOperationException">There is not enough space in <paramref name="data"/> 
        /// (starting from <paramref name="offset"/>) to hold all the data.</exception>
        [DebuggerStepThrough]
        public void CopyTo(T[] data, int offset) => CopyTo(data, offset, Count);

        /// <summary>
        /// Copy data from the <see cref="FragmentBuffer{T}"/> to an array.
        /// </summary>
        /// <param name="data">The array to copy into.</param>
        /// <param name="offset">The offset in <paramref name="data"/> at which to begin copying.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
        /// <exception cref="InvalidOperationException">There is not enough space in <paramref name="data"/> 
        /// (starting from <paramref name="offset"/>) to hold all the data.</exception>
        [DebuggerStepThrough]
        public void CopyTo(T[] data, int offset, int length)
        {
            if (length == 0)
                return;
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} must be larger or equal to zero.");
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} must be larger or equal to zero.");
            if (data.Length < length)
                throw new InvalidOperationException($"Not enough space in {nameof(data)}.");
            if (data.Length - offset < length)
                throw new InvalidOperationException($"Not enough space in {nameof(data)} with the provided {nameof(offset)}.");
            for (int i = 0; i < Count; ++i)
                data[offset + i] = this[i];
        }

        /// <summary>
        /// Creates an array from a <see cref="CircularBuffer{T}"/>.
        /// </summary>
        /// <returns>An array that contains the elements of the <see cref="CircularBuffer{T}"/> instance.</returns>
        [DebuggerStepThrough]
        public T[] ToArray()
        {
            var output = new T[Count];
            if (Count > 0)
                CopyTo(output, 0);
            return output;
        }

        #region Private fields
        readonly protected T[] _buffer;
        readonly protected int _offset;
        readonly protected int _count;
        #endregion Private fields
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
