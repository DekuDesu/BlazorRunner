using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public class TaskDirector : IDisposable
    {
        public static readonly IProducerConsumerCollection<DirectedTask> Queue = new ConcurrentQueue<DirectedTask>();

        private static IDictionary<Guid, CancellationTokenSource> TokenSources = new ConcurrentDictionary<Guid, CancellationTokenSource>();

        private static readonly CancellationTokenSource GlobalToken = new();

        private readonly SemaphoreSlim TaskLimiter = new(Environment.ProcessorCount, Environment.ProcessorCount);

        public int MaxRunningTasks { get; set; } = Environment.ProcessorCount;

        public event Action<Guid, TaskResult> OnTaskResult;
        public event Action<Guid> OnTaskQueued;
        public event Action<Guid> OnTaskStarted;

        public async Task ExecuteQueuedTasks()
        {
            while (true)
            {
                if (GlobalToken.IsCancellationRequested)
                {
                    return;
                }

                if (Queue.TryTake(out DirectedTask task))
                {
                    task.OnAny += (caller, args) => OnTaskResult?.Invoke(((DirectedTask)caller).Id, args);
                    OnTaskStarted?.Invoke(task.Id);
                    task?.BackingTask.Start();
                }

                await Task.Yield();
            }
        }

        public Guid QueueTask(Action action)
        {
            var (id, tokenSource) = CreateEmphemeralTokenSource();

            var newTask = new DirectedTask(action, TaskLimiter, tokenSource.Token) { Id = id };

            while (Queue.TryAdd(newTask) is false)
            {
                GlobalToken.Token.ThrowIfCancellationRequested();
            }

            OnTaskQueued?.Invoke(newTask.Id);

            return id;
        }

        public bool TryCancelTask(Guid TaskId)
        {
            if (TokenSources.ContainsKey(TaskId))
            {
                if (TokenSources.TryGetValue(TaskId, out var source))
                {
                    source?.Cancel();
                    return true;
                }
            }
            return false;
        }

        private (Guid Id, CancellationTokenSource Token) CreateEmphemeralTokenSource()
        {
            CancellationTokenSource newSource = new();

            Guid Id = Guid.NewGuid();

            newSource.Token.Register(() => TokenSources.Remove(Id));

            TokenSources.TryAdd(Id, newSource);

            return (Id, newSource);
        }

        public void Dispose()
        {
            GlobalToken?.Cancel();

            foreach (var item in TokenSources)
            {
                item.Value?.Cancel();
            }

            TokenSources.Clear();

            ((ConcurrentQueue<DirectedTask>)Queue).Clear();

            TaskLimiter?.Dispose();
        }
    }
}
