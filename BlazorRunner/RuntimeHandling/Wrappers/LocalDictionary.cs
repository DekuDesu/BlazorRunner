using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.RuntimeHandling
{
    public class LocalDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public string Path { get; set; } = $"{Guid.NewGuid()}.json";

        public LocalDictionary(string Path)
        {
            this.Path = Path;
        }

        public ConcurrentDictionary<TKey, TValue> BackingDictionary { get; private set; } = new ConcurrentDictionary<TKey, TValue>();

        public bool AutoSave { get; set; } = true;

        public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)BackingDictionary).Keys;

        public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)BackingDictionary).Values;

        public int Count => ((ICollection<KeyValuePair<TKey, TValue>>)BackingDictionary).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)BackingDictionary).IsReadOnly;

        public TValue this[TKey key] { get => ((IDictionary<TKey, TValue>)BackingDictionary)[key]; set => ((IDictionary<TKey, TValue>)BackingDictionary)[key] = value; }

        public virtual void Set(TKey Key, TValue Value) => Task.Run(() => SetAsync(Key, Value));

        public virtual async Task SetAsync(TKey Key, TValue Value)
        {
            if (BackingDictionary.ContainsKey(Key))
            {
                BackingDictionary[Key] = Value;
            }
            else
            {
                BackingDictionary.TryAdd(Key, Value);
            }
            if (AutoSave)
            {
                await SaveAsync();
            }
        }

        public virtual TValue Get(TKey Key)
        {
            if (BackingDictionary.ContainsKey(Key))
            {
                return BackingDictionary[Key];
            }
            return default;
        }

        public virtual void Save() => SaveAsync().Wait();

        public virtual async Task SaveAsync()
        {
            string saved = JsonConvert.SerializeObject(BackingDictionary, Formatting.Indented);

            Thread.BeginCriticalRegion();
            try
            {
                if (File.Exists(Path))
                {
                    File.Delete(Path);
                }

                using var writer = File.CreateText(Path);

                await writer.WriteAsync(saved);

                writer.Flush();

                writer.Dispose();
            }
            finally
            {
                Thread.EndCriticalRegion();
            }
        }

        public virtual bool Load() => LoadAsync().Result;

        public virtual async Task<bool> LoadAsync()
        {
            if (File.Exists(Path))
            {
                using var reader = File.OpenText(Path);

                string dict = await reader.ReadToEndAsync();

                BackingDictionary = JsonConvert.DeserializeObject<ConcurrentDictionary<TKey, TValue>>(dict);

                return true;
            }
            return false;
        }

        public void Add(TKey key, TValue value)
        {
            Set(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return ((IDictionary<TKey, TValue>)BackingDictionary).ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            bool removed = ((IDictionary<TKey, TValue>)BackingDictionary).Remove(key);
            if (AutoSave && removed)
            {
                Save();
            }
            return removed;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return ((IDictionary<TKey, TValue>)BackingDictionary).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)BackingDictionary).Add(item);
            if (AutoSave)
            {
                Save();
            }
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)BackingDictionary).Clear();
            if (AutoSave)
            {
                Save();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)BackingDictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)BackingDictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool removed = ((ICollection<KeyValuePair<TKey, TValue>>)BackingDictionary).Remove(item);
            if (AutoSave && removed)
            {
                Save();
            }
            return removed;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)BackingDictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)BackingDictionary).GetEnumerator();
        }
    }
}
