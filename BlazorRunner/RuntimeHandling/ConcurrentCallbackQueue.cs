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
    public class ConcurrentCallbackQueue<T> :
        IProducerConsumerCollection<T>, IDisposable
        where T : IEquatable<T>
    {
        private readonly ConcurrentQueue<T> BackingQueue = new();

        public event Action<object, T> OnPush;
        public event Action<object, T> OnPop;
        public event Action<object, T> OnRemove;
        public event Action<object> OnEmpty;
        public event Action<object, T> OnAny;

        public int Count => BackingQueue.Count;

        public bool IsEmpty => BackingQueue.IsEmpty;

        public ConcurrentCallbackQueue()
        {

        }

        private readonly EventWaitHandle WriteLock = new(true, EventResetMode.ManualReset);
        private readonly EventWaitHandle ReaderLock = new(true, EventResetMode.ManualReset);

        public bool Disposed { get; private set; } = false;

        public void Add(T item) => Push(item);

        public void Enqueue(T item) => Push(item);

        public bool Push(T item)
        {
            WriteLock.WaitOne();

            ReaderLock.Reset();

            try
            {
                BackingQueue.Enqueue(item);
            }
            finally
            {
                ReaderLock.Set();
            }

            OnPush?.Invoke(this, item);
            OnAny?.Invoke(this, item);


            return true;
        }

        public bool TryDequeue(out T item) => TryPop(out item);

        public bool TryPop([MaybeNullWhen(returnValue: false)] out T item)
        {
            if (BackingQueue.TryDequeue(out item))
            {
                if (Count is 0)
                {
                    OnEmpty?.Invoke(this);
                }

                OnPop?.Invoke(this, item);

                OnAny?.Invoke(this, item);

                return true;
            }
            return false;
        }

        public void Remove(T item)
        {
            if (BackingQueue.Contains(item) is false)
            {
                return;
            }
            if (BackingQueue.Count is 0)
            {
                return;
            }

            // when we remove something from the queue we should lock it becuase we have to do drastic changes to the queue
            WriteLock.Reset();

            // wait for any readers to finish before we adjust the queue
            ReaderLock.WaitOne();

            try
            {
                int count = Count - 1;
                int itemsPopped = 0;
                /*
                    remove 3    
                    1 2 3 4 5   count = 4
                    ↓
                    2 3 4 5 1   itemsPopped = 1
                    ↓
                    3 4 5 1 2   itemsPopped = 2
                    ↓
                    4 5 1 2 _   4 - 2 = 2
                    ↓
                    5 1 2 4 _   pop push 1
                    ↓
                    1 2 4 5 _   pop push 2
                */
                // pop and push until we find the element
                // when we find it break and throw out value
                while (true)
                {
                    if (BackingQueue.TryDequeue(out T tmp))
                    {
                        if (item.Equals(tmp))
                        {
                            break;
                        }
                        else
                        {
                            BackingQueue.Enqueue(tmp);
                            itemsPopped++;
                        }
                    }
                }

                // after we found the item we should re-order the queue to match the original order(minus the element that was
                // removed)
                // just pop push until original order restored
                for (int i = 0; i < count - itemsPopped; i++)
                {
                    if (BackingQueue.TryDequeue(out T tmp))
                    {
                        BackingQueue.Enqueue(tmp);
                    }
                }
            }
            finally
            {
                // make sure to release the lock even if we encounter an error
                WriteLock.Set();

                OnRemove?.Invoke(this, item);
                OnAny?.Invoke(this, item);
            }
        }

        public void Clear()
        {
            // when we remove something from the queue we should lock it becuase we have to do drastic changes to the queue
            WriteLock.Reset();

            // wait for any readers to finish before we adjust the queue
            ReaderLock.WaitOne();

            try
            {
                BackingQueue.Clear();

                OnEmpty?.Invoke(this);
            }
            finally
            {
                WriteLock.Set();
            }
        }

        public bool IsSynchronized => ((ICollection)BackingQueue).IsSynchronized;

        public object SyncRoot => ((ICollection)BackingQueue).SyncRoot;

        public void CopyTo(T[] array, int index) => BackingQueue.CopyTo(array, index);

        public void CopyTo(Array array, int index) => ((ICollection)BackingQueue).CopyTo(array, index);

        public IEnumerator<T> GetEnumerator() => BackingQueue.GetEnumerator();

        public T[] ToArray() => BackingQueue.ToArray();

        public bool TryAdd(T item) => Push(item);

        public bool TryTake([MaybeNullWhen(false)] out T item) => BackingQueue.TryDequeue(out item);

        IEnumerator IEnumerable.GetEnumerator() => BackingQueue.GetEnumerator();

        public void Dispose()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(ConcurrentCallbackQueue<T>));
            }

            Disposed = true;

            ((IDisposable)WriteLock)?.Dispose();
            ((IDisposable)ReaderLock)?.Dispose();
        }
    }
}
