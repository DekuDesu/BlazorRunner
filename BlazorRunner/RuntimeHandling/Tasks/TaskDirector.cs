using BlazorRunner.Runner.RuntimeHandling;
using Microsoft.Extensions.Logging;
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

        public static SemaphoreSlim TaskLimiter { get; private set; } = new(Environment.ProcessorCount, Environment.ProcessorCount);

        /// <summary>
        /// the amount of queued tasks
        /// </summary>
        public static int Queued => QueuedTasks.Count;

        /// <summary>
        /// The amount of running tasks
        /// </summary>
        public static int Running => RunningTasks.Count;

        /// <summary>
        /// The limit of tasks that can be executed at once, use <see cref="ResizeCapacity"/> to change the size during runtime.
        /// </summary>
        public static int Capacity { get; private set; } = Environment.ProcessorCount;

        /// <summary>
        /// Returns true when tasks are waiting to be executed from the queue
        /// </summary>
        public static bool ExecutingTasks { get; private set; } = false;

        /// <summary>
        /// <see langword="true"/> when no queued or running tasks exist
        /// </summary>
        public static bool IsEmpty => QueuedTasks.IsEmpty && RunningTasks.IsEmpty;

        private static CancellationTokenSource GlobalToken = new();

        private static int NewCapacity = Environment.ProcessorCount;

        private static bool ChangingCapacity = false;

        static TaskDirector()
        {
            QueuedTasks.OnPush += async (x, y) => await ExecuteQueuedTasks();
        }

        public static async Task ExecuteQueuedTasks()
        {
            ExecutingTasks = true;

            while (QueuedTasks.IsEmpty is false)
            {
                // make sure we can exit gracefully
                if (GlobalToken.IsCancellationRequested)
                {
                    ExecutingTasks = false;
                    return;
                }

                if (await TaskLimiter.WaitAsync(1))
                {
                    if (QueuedTasks.TryDequeue(out DirectedTask task))
                    {
                        // execute it
                        task?.BackingTask.Start();
                    }
                }

                await Task.Delay(10);
            }
        }

        public static async Task<DirectedTask> QueueTask(Action<CancellationToken> action, Guid? Id, ILogger logger, string Name)
        {
            return await Task.Run(() => QueueWorker(action, Id, logger, Name), GlobalToken.Token);
        }

        private static DirectedTask QueueWorker(Action<CancellationToken> action, Guid? Id, ILogger logger, string Name)
        {
            var id = Id ?? Guid.NewGuid();

            var newTask = new DirectedTask(action, TaskLimiter, logger, Name) { BackingId = id };

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

        public static void ResizeCapacity(int Capacity)
        {
            if (Capacity == TaskDirector.Capacity)
            {
                return;
            }

            if (ChangingCapacity is false)
            {
                NewCapacity = Capacity;

                if (IsEmpty)
                {
                    ResizeSemaphore();
                    return;
                }

                ChangingCapacity = true;

                RunningTasks.OnEmpty += Resize;
                QueuedTasks.OnEmpty += Resize;
            }
        }

        private static void Resize<T>(T caller)
        {
            if (TaskLimiter.CurrentCount == Capacity && RunningTasks.IsEmpty && QueuedTasks.IsEmpty)
            {
                ResizeSemaphore();

                RunningTasks.OnEmpty -= Resize;
                QueuedTasks.OnEmpty -= Resize;

                ChangingCapacity = false;
            }
        }

        private static void ResizeSemaphore()
        {
            for (int i = 0; i < Capacity; i++)
            {
                TaskLimiter.Wait();
            }
            TaskLimiter?.Dispose();
            TaskLimiter = new SemaphoreSlim(NewCapacity, NewCapacity);
            Capacity = NewCapacity;
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
