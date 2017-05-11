using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CloudinaryDotNet
{
    /// <summary>
    /// This class is based on list so is very slow but allows not unique keys.
    /// This behavior is required for DeleteResources and ListResources commands.
    /// </summary>
    public class StringDictionary : IEnumerable<KeyValuePair<string, string>>, IDictionary<string, string>
    {
        List<KeyValuePair<string, string>> m_list = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StringDictionary() { }

        /// <summary>
        /// Constructs a new instance from an array of strings.
        /// </summary>
        /// <param name="keyValuePairs">Array of strings in form of "key=value". A string could also contain only a key ("key"). Only the first '=' character is used to split string.</param>
        public StringDictionary(params string[] keyValuePairs)
        {
            foreach (var pair in keyValuePairs)
            {
                var firstEq = pair.IndexOf('=');
                if (firstEq == -1)
                {
                    Add(pair, null);
                }
                else
                {
                    Add(pair.Substring(0, firstEq), pair.Substring(firstEq + 1));
                }
            }
        }

        /// <summary>
        /// Whether the list should be sorted before enumerating.
        /// </summary>
        public bool Sort { get; set; }

        /// <summary>
        /// Add a new pair of key and value.
        /// </summary>
        public void Add(string key, string value)
        {
            var newItem = new KeyValuePair<string, string>(key, value);
            m_list.Add(newItem);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string Remove(string key)
        {
            foreach (var item in m_list)
            {
                if (item.Key == key)
                {
                    m_list.Remove(item);
                    return item.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a value of a first found key or adds a new pair of key and value or updates a value of first found key.
        /// </summary>
        public string this[string key]
        {
            get
            {
                foreach (var item in m_list)
                {
                    if (item.Key == key)
                        return item.Value;
                }

                return null;
            }
            set
            {
                KeyValuePair<string, string> newItem = new KeyValuePair<string, string>(key, value);
                bool updated = false;
                for (int i = 0; i < m_list.Count; i++)
                {
                    if (m_list[i].Key == key)
                    {
                        m_list[i] = newItem;
                        updated = true;
                    }
                }

                if (!updated)
                    m_list.Add(newItem);
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            m_list.Clear();
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public int Count
        {
            get
            {
                return m_list.Count;
            }
        }

        /// <summary>
        /// Returns all keys and values.
        /// </summary>
        public string[] Pairs
        {
            get
            {
                return m_list.Select(pair => pair.Value == null
                    ? pair.Key
                    : String.Format("{0}={1}", pair.Key, pair.Value)).ToArray();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            if (Sort)
            {
                var sorted = new SortedList<string, string>(this);
                return sorted.GetEnumerator();
            }
            else
            {
                return m_list.GetEnumerator();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, false.
        /// </returns>
        public bool ContainsKey(string key)
        {
            foreach (var item in m_list)
            {
                if (item.Key == key)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public ICollection<string> Keys
        {
            get { return m_list.Select(pair => pair.Key).ToArray(); }
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        bool IDictionary<string, string>.Remove(string key)
        {
            foreach (var item in m_list)
            {
                if (item.Key == key)
                {
                    m_list.Remove(item);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetValue(string key, out string value)
        {
            value = null;

            foreach (var item in m_list)
            {
                if (item.Key == key)
                {
                    value = item.Value;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public ICollection<string> Values
        {
            get { return m_list.Select(pair => pair.Value).ToArray(); }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(KeyValuePair<string, string> item)
        {
            m_list.Add(item);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<string, string> item)
        {
            return m_list.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            m_list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(KeyValuePair<string, string> item)
        {
            return m_list.Remove(item);
        }
    }
}
