using ftrip.io.framework.Utilities.Contracts;
using System;
using System.Collections.Generic;

namespace ftrip.io.framework.Utilities
{
    public class DefaultDictionary<TKey, TValue> : IDefaultDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary;

        public DefaultDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public TValue this[TKey key]
        {
            get
            {
                if (!_dictionary.ContainsKey(key))
                {
                    _dictionary[key] = (TValue)Activator.CreateInstance(typeof(TValue));
                }

                return _dictionary[key];
            }
            set => _dictionary[key] = value;
        }

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public IDictionary<TKey, TValue> ToDictionary()
        {
            return _dictionary;
        }
    }
}