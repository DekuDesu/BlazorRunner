using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public static class TaskDirector
    {
        public static readonly ConcurrentQueue<DirectedTask> Queue = new ConcurrentQueue<DirectedTask>();

        public static IReadOnlyList<DirectedTask> Running => _Running;

        private static readonly List<DirectedTask> _Running = new();

        private static readonly IDictionary<DirectedTask, CancellationTokenSource> TokenSources = new ConcurrentDictionary<DirectedTask, CancellationTokenSource>();

        private static CancellationTokenSource GlobalToken = new();

        public static readonly SemaphoreSlim TaskLimiter = new(Environment.ProcessorCount, Environment.ProcessorCount);

        public static int RunningTasks => MaxRunningTasks - TaskLimiter.CurrentCount;

        public static int MaxRunningTasks { get; set; } = Environment.ProcessorCount;

        public static event Action<Guid, TaskResult> OnTaskResult;
        public static event Action<Guid> OnTaskQueued;
        public static event Action<Guid> OnTaskStarted;
        public static event Action<Guid> OnTaskFinished;
        public static event Action<Guid> OnTaskAny;

        public static bool ExecutingTasks { get; private set; } = false;

        public static async Task ExecuteQueuedTasks()
        {
            ExecutingTasks = true;

            while (true)
            {
                // make sure we can exit gracefully
                if (GlobalToken.IsCancellationRequested)
                {
                    ExecutingTasks = false;
                    return;
                }

                // only bother if there are things to execute
                if (Queue.IsEmpty is false)
                {
                    // deque a task
                    if (Queue.TryDequeue(out DirectedTask task))
                    {
                        // execute it
                        await StartTask(task);
                    }
                }

                await Task.Delay(1);
            }
        }

        private static async Task StartTask(DirectedTask task)
        {
            while (true)
            {
                // make sure we can exit gracefully
                if (GlobalToken.IsCancellationRequested)
                {
                    return;
                }
                // wait for sempahore for only 1 ms
                if (await TaskLimiter.WaitAsync(1))
                {
                    // start the task if we got the sempahore
                    task?.BackingTask.Start();
                    return;
                }
            }
        }

        public static void QueueTask(Action action, Guid? Id)
        {
            Task.Run(() => QueueWorker(action, Id), GlobalToken.Token);
        }

        private static DirectedTask QueueWorker(Action action, Guid? Id)
        {
            // create a token so we can cancel the task at any time
            CancellationTokenSource newSource = new();

            var id = Id ?? Guid.NewGuid();

            var newTask = new DirectedTask(action, TaskLimiter, newSource.Token) { Id = id };

            // register a callback so we remove it when it gets cancelled
            newSource.Token.Register(() => TokenSources.Remove(newTask));

            // add it to the dict
            TokenSources.TryAdd(newTask, newSource);

            // make sure we register call backs for the task so we can keep track of it
            newTask.OnFinal += (x, y) => RemoveRunningTask((DirectedTask)x);
            newTask.OnAny += (caller, args) => OnTaskResult?.Invoke(id, args);
            newTask.OnStart += (x, y) => AddRunningTask(newTask);

            // queue it for execution
            Queue.Enqueue(newTask);

            // notify subscribers we've changed collections
            OnTaskQueued?.Invoke(newTask.Id);
            OnTaskAny?.Invoke(newTask.Id);

            return newTask;
        }

        public static bool TryCancelTask(DirectedTask task)
        {
            if (TokenSources.ContainsKey(task))
            {
                if (TokenSources.TryGetValue(task, out var source))
                {
                    source?.Cancel();

                    RemoveRunningTask(task);

                    task.NotifyExternalCancelled();

                    return true;
                }
            }
            return false;
        }

        private static void AddRunningTask(DirectedTask task)
        {
            lock (_Running)
            {
                _Running.Add(task);
                OnTaskStarted?.Invoke(task.Id);
                OnTaskAny?.Invoke(task.Id);
            }
        }

        private static void RemoveRunningTask(DirectedTask task)
        {
            lock (_Running)
            {
                _Running.Remove(task);
            }

            OnTaskFinished?.Invoke(task.Id);
            OnTaskAny?.Invoke(task.Id);
        }

        public static void Dispose()
        {
            GlobalToken?.Cancel();

            GlobalToken?.Dispose();

            GlobalToken = new CancellationTokenSource();

            foreach (var item in TokenSources)
            {
                item.Value?.Cancel();
            }

            TokenSources.Clear();

            ((ConcurrentQueue<DirectedTask>)Queue).Clear();

        }
    }
}
