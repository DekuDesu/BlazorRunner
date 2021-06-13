using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.RuntimeHandling
{
    public class ConcurrentCallbackDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> BackingDictionary = new ConcurrentDictionary<TKey, TValue>();

        public TValue this[TKey key] { get => BackingDictionary[key]; set => UpdateValue(key, value); }

        public ICollection<TKey> Keys => BackingDictionary.Keys;

        public ICollection<TValue> Values => BackingDictionary.Values;

        public int Count => BackingDictionary.Count;

        public bool IsEmpty => Count == 0;

        public bool IsReadOnly => BackingDictionary.IsReadOnly;

        public event Action<object, KeyValuePair<TKey, TValue>> OnAdd;
        /// <summary>
        /// Bind an event that accepts params this object, the key that was updated, the old value, and the new value
        /// </summary>
        public event Action<object, TKey, TValue, TValue> OnUpdate;
        public event Action<object, TKey> OnRemove;
        public event Action<object> OnEmpty;
        public event Action<object, TKey> OnAny;

        public void UpdateValue(TKey Key, TValue Value)
        {
            TValue previous = BackingDictionary[Key];

            BackingDictionary[Key] = Value;

            OnUpdate?.Invoke(this, Key, previous, Value);
            OnAny?.Invoke(this, Key);
        }

        public void Add(TKey key, TValue value)
        {
            BackingDictionary.Add(key, value);
            OnAdd?.Invoke(this, new KeyValuePair<TKey, TValue>(key, value));
            OnAny?.Invoke(this, key);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            BackingDictionary.Add(item);
            OnAdd?.Invoke(this, item);
            OnAny?.Invoke(this, item.Key);
        }

        public void Clear() => BackingDictionary.Clear();

        public bool Contains(KeyValuePair<TKey, TValue> item) => BackingDictionary.Contains(item);

        public bool ContainsKey(TKey key) => BackingDictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => BackingDictionary.CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => BackingDictionary.GetEnumerator();

        public bool Remove(TKey key)
        {
            bool removed = BackingDictionary.Remove(key);

            if (BackingDictionary.Count is 0)
            {
                OnEmpty?.Invoke(this);
            }

            OnRemove?.Invoke(this, key);

            OnAny?.Invoke(this, key);

            return removed;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool removed = BackingDictionary.Remove(item);

            if (BackingDictionary.Count is 0)
            {
                OnEmpty?.Invoke(this);
            }

            OnRemove?.Invoke(this, item.Key);

            OnAny?.Invoke(this, item.Key);

            return removed;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => BackingDictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => BackingDictionary.GetEnumerator();
    }
}
