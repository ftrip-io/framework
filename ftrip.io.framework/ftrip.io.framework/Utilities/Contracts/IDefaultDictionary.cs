using System.Collections.Generic;

namespace ftrip.io.framework.Utilities.Contracts
{
    public interface IDefaultDictionary<TKey, TValue>
    {
        TValue this[TKey key] { get; set; }

        ICollection<TKey> Keys { get; }
        ICollection<TValue> Values { get; }

        void Add(TKey key, TValue value);

        bool ContainsKey(TKey key);

        bool Remove(TKey key);

        bool TryGetValue(TKey key, out TValue value);

        IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

        IDictionary<TKey, TValue> ToDictionary();
    }
}