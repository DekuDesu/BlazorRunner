using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public class DirectedTask
    {
        public readonly Task BackingTask;

        public readonly Stopwatch Timer = new();

        private readonly Action BackingAction;

        private readonly SemaphoreSlim Limiter;

        private object LimiterLock = new();

        private bool ReleasedLimiter = false;

        public Guid Id { get; init; }

        public Exception Fault { get; private set; } = null;

        public DirectedTaskStatus Status { get; private set; } = DirectedTaskStatus.Queued;

        public event Action<object, TaskResult> OnStart;
        public event Action<object, TaskResult> OnComplete;
        public event Action<object, TaskResult> OnFault;
        public event Action<object, TaskResult> OnCancel;
        public event Action<object, TaskResult> OnAny;
        public event Action<object, TaskResult> OnFinal;


        public DirectedTask(Action Work, SemaphoreSlim Limiter, CancellationToken Token)
        {
            this.BackingAction = Work;

            this.Limiter = Limiter;

            this.BackingTask = new Task(Worker, Token);
        }

        private void Worker()
        {
            var result = new TaskResult();

            try
            {
                OnStart?.Invoke(this, result);
                OnAny?.Invoke(this, result);

                Status = DirectedTaskStatus.Running;

                Timer.Start();

                BackingAction?.Invoke();

                Timer.Stop();

                Status = DirectedTaskStatus.Finished;

                result.TimeTaken = Timer.ElapsedMilliseconds;

                OnComplete?.Invoke(this, result);
                OnAny?.Invoke(this, result);
            }
            catch (Exception e)
            {
                Timer.Stop();

                Fault = e;

                Status = DirectedTaskStatus.Faulted;

                result.TimeTaken = Timer.ElapsedMilliseconds;
                result.Fault = e;

                OnFault?.Invoke(this, result);
                OnAny?.Invoke(this, result);
            }
            finally
            {
                ReleaseSemaphore();

                OnFinal?.Invoke(this, result);
            }
        }

        public void NotifyExternalCancelled()
        {
            Timer?.Stop();

            // release sempahore so other queued tasks can continue work
            ReleaseSemaphore();

            // create and notify subscribers
            var result = new TaskResult();
            result.Cancelled = true;
            result.TimeTaken = Timer.ElapsedMilliseconds;

            // set our status so it updates in UX
            Status = DirectedTaskStatus.Cancelled;

            OnCancel?.Invoke(this, result);
            OnAny?.Invoke(this, result);
            OnFinal?.Invoke(this, result);
        }

        private void ReleaseSemaphore()
        {
            if (ReleasedLimiter is false)
            {
                lock (LimiterLock)
                {
                    Limiter?.Release();
                    ReleasedLimiter = true;
                }
            }
        }
    }
}
