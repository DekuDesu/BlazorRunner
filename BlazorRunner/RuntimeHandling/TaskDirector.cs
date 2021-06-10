using BlazorRunner.Runner.RuntimeHandling;
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
        public static readonly ConcurrentCallbackQueue<DirectedTask> QueuedTasks = new();

        public static readonly ConcurrentCallbackDictionary<Guid, DirectedTask> RunningTasks = new();

        private static CancellationTokenSource GlobalToken = new();

        public static readonly SemaphoreSlim TaskLimiter = new(Environment.ProcessorCount, Environment.ProcessorCount);

        public static int RunningCount => MaxRunningTasks - TaskLimiter.CurrentCount;

        public static int MaxRunningTasks { get; set; } = Environment.ProcessorCount;

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
                if (QueuedTasks.IsEmpty is false)
                {
                    // deque a task
                    if (QueuedTasks.TryDequeue(out DirectedTask task))
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
            var id = Id ?? Guid.NewGuid();

            var newTask = new DirectedTask(action, TaskLimiter, new()) { BackingId = id };

            // make sure we register call backs for the task so we can keep track of it
            newTask.OnFinal += (x, y) => RemoveRunningTask((DirectedTask)x);
            newTask.OnStart += (x, y) => AddRunningTask(newTask);

            // queue it for execution
            QueuedTasks.Enqueue(newTask);

            return newTask;
        }

        private static void AddRunningTask(DirectedTask task)
        {
            RunningTasks.Add(task.Id, task);
        }

        private static void RemoveRunningTask(DirectedTask task)
        {
            RunningTasks?.Remove(task.Id);
        }

        public static void Dispose()
        {
            GlobalToken?.Cancel();

            GlobalToken?.Dispose();

            GlobalToken = new CancellationTokenSource();

            QueuedTasks.Clear();

        }
    }
}
