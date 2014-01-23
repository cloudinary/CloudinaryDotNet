using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CloudinaryDotNet
{
    /// <summary>
    /// This dictionary is based on list so is not very fast but allows not unique keys
    /// </summary>
    public class StringDictionary : IEnumerable<KeyValuePair<string, string>>
    {
        List<KeyValuePair<string, string>> m_list = new List<KeyValuePair<string, string>>();

        public StringDictionary() { }

        public StringDictionary(params string[] keyValuePairs)
        {
            foreach (var pair in keyValuePairs)
            {
                string[] splittedPair = pair.Split('=');
                if (splittedPair.Length != 2)
                    throw new ArgumentException(String.Format("Couldn't parse '{0}'!", pair));

                Add(splittedPair[0], splittedPair[1]);
            }
        }

        public void Add(string key, string value)
        {
            KeyValuePair<string, string> newItem = new KeyValuePair<string, string>(key, value);
            m_list.Add(newItem);
        }

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

        public void Clear()
        {
            m_list.Clear();
        }

        public int Count
        {
            get
            {
                return m_list.Count;
            }
        }

        public string[] Keys
        {
            get
            {
                return m_list.Select(pair => pair.Key).ToArray();
            }
        }

        public string[] Values
        {
            get
            {
                return m_list.Select(pair => pair.Value).ToArray();
            }
        }

        public string[] Pairs
        {
            get
            {
                return m_list.Select(pair => String.Format("{0}={1}", pair.Key, pair.Value)).ToArray();
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_list.GetEnumerator();
        }
    }
}
