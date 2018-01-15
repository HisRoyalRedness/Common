using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

/*
    Bimap implementation.
    A dictionary that keys elements by name and value.

    Keith Fletcher
    Jan 2018

    This file is Unlicensed.
    See the foot of the file, or refer to <http://unlicense.org>
*/

namespace HisRoyalRedness.com
{
    #region BiMap<T>
    public class BiMap<T> : IDictionary<T, T>
    {
        readonly Dictionary<T, T> _dict1 = new Dictionary<T, T>();
        readonly Dictionary<T, T> _dict2 = new Dictionary<T, T>();

        #region IDictionary<T, T> implementation
        public void Add(T key, T value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _dict1.Add(key, value);
            try
            {
                _dict2.Add(value, key);
            }
            catch (Exception)
            {
                _dict1.Remove(key);
                throw;
            }
        }

        public void Add(KeyValuePair<T, T> item) => Add(item.Key, item.Value);

        public bool Remove(T key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (_dict1.ContainsKey(key))
            {
                var value = _dict1[key];
                if (!_dict1.Remove(key))
                    return false;

                if (!_dict2.Remove(value))
                    throw new ApplicationException(
                        string.Format(
                            "The {0} is in an inconsistent state after attempting to remove an item.",
                            GetTypeName()));
                return true;
            }

            else if (_dict2.ContainsKey(key))
            {
                var value = _dict2[key];
                if (!_dict2.Remove(key))
                    return false;

                if (!_dict1.Remove(value))
                    throw new ApplicationException(
                        string.Format(
                            "The {0} is in an inconsistent state after attempting to remove an item.",
                            GetTypeName()));
                return true;
            }

            else
                return false;
        }

        public bool Remove(KeyValuePair<T, T> item) => Remove(item.Key);

        public T this[T key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                return (_dict1.ContainsKey(key))
                    ? _dict1[key]
                    : _dict2[key];
            }
            set
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                // Key exists, but value points to a different key
                // eg 1 = A
                //    2 = B
                //    3 = C
                //
                // dict[1] = C or dict[4] = C

                if (ContainsKey(key) && (ContainsKey(value)) && !_dict1[key].Equals(value))
                    throw new ArgumentException("An element with the same value already exists in the {0}.", GetTypeName());
                
                // Existing key
                if (ContainsKey(key))
                {
                    Debug.Assert(Remove(_dict1[key]));
                    Remove(key);

                }

                // Different key, but existing value
                else if (ContainsKey(value))
                {
                    // Key exists, but value points to a different key
                    // eg 1 = A
                    //    2 = B
                    //    3 = C
                    //
                    // dict[4] = C
                    throw new ArgumentException("An element with the same value already exists in the Dictionary.");
                }

                Add(key, value);
            }
        }

        public bool ContainsKey(T key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            return _dict1.ContainsKey(key);
        }

        public bool Contains(KeyValuePair<T, T> item) => _dict1.Contains(item);

        public ICollection<T> Keys
        { get { return _dict1.Keys; } }

        public ICollection<T> Values
        { get { return _dict1.Values; } }

        public void Clear()
        {
            _dict1.Clear();
            _dict2.Clear();
        }

        public int Count
        { get { return _dict1.Count; } }

        public bool IsReadOnly
        { get { return IDict1.IsReadOnly || IDict2.IsReadOnly; } }

        public IEnumerator<KeyValuePair<T, T>> GetEnumerator()
        { return _dict1.GetEnumerator(); }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        { return this.GetEnumerator(); }

        public bool TryGetValue(T key, out T value)
        {
            if (!_dict1.TryGetValue(key, out value))
                return _dict2.TryGetValue(key, out value);
            
            return true;
        }

        public void CopyTo(KeyValuePair<T, T>[] array, int arrayIndex)
        {
            IDict1.CopyTo(array, arrayIndex);
        }
        #endregion IDictionary<T1, T2> implementation

        IDictionary<T, T> IDict1
        { get { return (IDictionary<T, T>)_dict1; } }

        IDictionary<T, T> IDict2
        { get { return (IDictionary<T, T>)_dict2; } }

        string GetTypeName(bool getFullName = false)
        {
            Func<Type, string> getName = t => getFullName ? t.FullName : t.Name;

            var type = GetType();
            var typeArgs = type.GetGenericArguments();
            return string.Format("{0}<{1}>", getName(type).Remove(getName(type).IndexOf('`')), getName(typeArgs[0]));
        }
    }
    #endregion BiMap<T>

    #region BiMap<T1, T2>
    public class BiMap<T1, T2> : IDictionary<T1, T2>
    {
        public BiMap()
        {
            _dict1 = new Dictionary<T1, T2>();
            _dict2 = new Dictionary<T2, T1>();
        }

        public BiMap(int capacity)
        {
            _dict1 = new Dictionary<T1, T2>(capacity);
            _dict2 = new Dictionary<T2, T1>(capacity);
        }

        public BiMap(IEqualityComparer<T1> keyComparer, IEqualityComparer<T2> valueComparer)
        {
            _dict1 = new Dictionary<T1, T2>(keyComparer);
            _dict2 = new Dictionary<T2, T1>(valueComparer);
        }

        public BiMap(IDictionary<T1, T2> dictionary)
            : this()
        {
            foreach (var item in dictionary)
                Add(item.Key, item.Value);
        }

        public BiMap(int capacity, IEqualityComparer<T1> keyComparer, IEqualityComparer<T2> valueComparer)
        {
            _dict1 = new Dictionary<T1, T2>(capacity, keyComparer);
            _dict2 = new Dictionary<T2, T1>(capacity, valueComparer);
        }

        public BiMap(IDictionary<T1, T2> dictionary, IEqualityComparer<T1> keyComparer, IEqualityComparer<T2> valueComparer)
            : this(keyComparer, valueComparer)
        {
            foreach (var item in dictionary)
                Add(item.Key, item.Value);
        }

        #region IDictionary<T1, T2> implementation
        public void Add(T1 key, T2 value)
        {

            _dict1.Add(key, value);
            try
            {
                _dict2.Add(value, key);
            }
            catch (Exception)
            {
                _dict1.Remove(key);
                throw;
            }
        }

        public void Add(KeyValuePair<T1, T2> item) => Add(item.Key, item.Value);

        public bool Remove(T1 key)
        {

            if (!_dict1.ContainsKey(key))
                return false;

            var value = _dict1[key];
            if (!_dict1.Remove(key))
                return false;

            if (!_dict2.Remove(value))
                throw new ApplicationException($"The {GetTypeName()} is in an inconsistent state after attempting to remove an item.");

            return true;
        }

        public bool Remove(KeyValuePair<T1, T2> item) => Remove(item.Key);

        public T2 this[T1 key]
        {
            get { return _dict1[key]; }
            set
            {
                // Key exists, but value points to a different key
                // eg 1 = A
                //    2 = B
                //    3 = C
                //
                // dict[1] = C or dict[4] = C
                if (ContainsKey(key) && ContainsKey(value) && !_dict2[value].Equals(key))
                    throw new ArgumentException($"An element with the same value already exists in the {GetTypeName()}.");

                // Existing key
                if (ContainsKey(key))
                {
                    Debug.Assert(Remove(_dict1[key]));
                    Remove(key);

                }

                // Different key, but existing value
                else if (ContainsKey(value))
                {
                    // Key exists, but value points to a different key
                    // eg 1 = A
                    //    2 = B
                    //    3 = C
                    //
                    // dict[4] = C
                    throw new ArgumentException("An element with the same value already exists in the Dictionary.");
                }

                Add(key, value);
            }
        }

        public bool ContainsKey(T1 key) => _dict1.ContainsKey(key); 
        public bool Contains(KeyValuePair<T1, T2> item)=> _dict1.Contains(item);
        public ICollection<T1> Keys => _dict1.Keys;
        public ICollection<T2> Values => _dict1.Values; 
        public void Clear()
        {
            _dict1.Clear();
            _dict2.Clear();
        }

        public int Count => _dict1.Count;
        public bool IsReadOnly => IDict1.IsReadOnly || IDict2.IsReadOnly;
        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() => _dict1.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();
        public bool TryGetValue(T1 key, out T2 value) => _dict1.TryGetValue(key, out value);
        public void CopyTo(KeyValuePair<T1, T2>[] array, int arrayIndex) => IDict1.CopyTo(array, arrayIndex);        
        #endregion IDictionary<T1, T2> implementation

        #region IDictionary<T2, T1> implementation
        public void Add(T2 key, T1 value)
        {
            _dict2.Add(key, value);
            try
            {
                _dict1.Add(value, key);
            }
            catch (Exception)
            {
                _dict2.Remove(key);
                throw;
            }
        }

        public void Add(KeyValuePair<T2, T1> item) => Add(item.Key, item.Value);

        public bool Remove(T2 key)
        {
            if (!_dict2.ContainsKey(key))
                return false;

            var value = _dict2[key];
            if (!_dict2.Remove(key))
                return false;

            if (!_dict1.Remove(value))
                throw new ApplicationException($"The {GetTypeName()} is in an inconsistent state after attempting to remove an item.");

            return true;
        }

        public bool Remove(KeyValuePair<T2, T1> item) => Remove(item.Key);

        public T1 this[T2 key]
        {
            get { return _dict2[key]; }
            set
            {
                // Key exists, but value points to a different key
                // eg 1 = A
                //    2 = B
                //    3 = C
                //
                // dict[1] = C or dict[4] = C
                if (ContainsKey(key) && ContainsKey(value) && !_dict1[value].Equals(key))
                    throw new ArgumentException($"An element with the same value already exists in the {GetTypeName()}.");

                // Existing key
                if (ContainsKey(key))
                {
                    Debug.Assert(Remove(_dict2[key]));
                    Remove(key);

                }

                // Different key, but existing value
                else if (ContainsKey(value))
                {
                    // Key exists, but value points to a different key
                    // eg 1 = A
                    //    2 = B
                    //    3 = C
                    //
                    // dict[4] = C
                    throw new ArgumentException("An element with the same value already exists in the Dictionary.");
                }

                Add(key, value);
            }
        }

        public bool ContainsKey(T2 key) => _dict2.ContainsKey(key);
        public bool Contains(KeyValuePair<T2, T1> item) => _dict2.Contains(item);
        public bool TryGetValue(T2 key, out T1 value) => _dict2.TryGetValue(key, out value);
        public void CopyTo(KeyValuePair<T2, T1>[] array, int arrayIndex) => IDict2.CopyTo(array, arrayIndex);
        #endregion IDictionary<T2, T1> implementation

        IDictionary<T1, T2> IDict1 => (IDictionary<T1, T2>)_dict1;
        IDictionary<T2, T1> IDict2 => (IDictionary<T2, T1>)_dict2;

        string GetTypeName(bool getFullName = false)
        {
            Func<Type, string> getName = t => getFullName ? t.FullName : t.Name;
            var type = GetType();
            var typeArgs = type.GetGenericArguments();
            return $"{getName(type).Remove(getName(type).IndexOf('`'))}<{getName(typeArgs[0])},{getName(typeArgs[1])}>";
        }

        readonly Dictionary<T1, T2> _dict1;
        readonly Dictionary<T2, T1> _dict2;
    }
    #endregion BiMap<T1, T2>
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
