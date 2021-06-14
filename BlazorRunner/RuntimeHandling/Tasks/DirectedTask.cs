using BlazorRunner.Runner.RuntimeHandling;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlazorRunner.Runner.Helpers.Formatting;

namespace BlazorRunner.Runner
{
    public class DirectedTask : RegisteredObject, IEquatable<DirectedTask>, IDisposable
    {
        public readonly Task BackingTask;

        public string Name { get; init; }

        public readonly Stopwatch Timer = new();

        private readonly Action<CancellationToken> BackingAction;

        private readonly SemaphoreSlim Limiter;

        private readonly CancellationTokenSource TokenSource;

        private readonly object LimiterLock = new();

        private readonly ILogger Logger;

        private bool ReleasedLimiter = false;

        public bool Cancelled { get; private set; } = false;

        public bool Disposed { get; private set; } = false;

        public Guid BackingId { get; set; }

        public Exception Fault { get; private set; } = null;

        public DirectedTaskStatus Status { get; private set; } = DirectedTaskStatus.Queued;

        public event Action<object, TaskResult> OnStart;
        public event Action<object, TaskResult> OnComplete;
        public event Action<object, TaskResult> OnFault;
        public event Action<object, TaskResult> OnCancel;
        public event Action<object, TaskResult> OnAny;
        public event Action<object, TaskResult> OnFinal;


        public DirectedTask(Action<CancellationToken> Work, SemaphoreSlim Limiter, ILogger Logger, string Name)
        {
            this.BackingAction = Work;

            this.Limiter = Limiter;

            this.TokenSource = new();

            this.Logger = Logger;

            this.BackingTask = new Task(Worker, TokenSource.Token);

            this.Name = $"[{Name}]".AsMethodName();

            Logger.LogTrace($"Created worker {this.Name}");
        }

        private void Worker()
        {
            var result = new TaskResult();

            try
            {
                Status = DirectedTaskStatus.Running;

                OnStart?.Invoke(this, result);
                OnAny?.Invoke(this, result);
                Logger.LogTrace($"Started Worker {Name} {GetThreadInfo()}");



                Timer.Start();

                BackingAction?.Invoke(TokenSource.Token);

                Timer.Stop();

                if (Cancelled)
                {
                    // if the task was cancelled but we got here - then the backing action did not throw when the token ended
                    Logger.LogWarning(Helpers.Warnings.DirectedTaskCancelledWithoutThrowing());
                }

                Status = DirectedTaskStatus.Finished;

                result.TimeTaken = Timer.ElapsedMilliseconds;

                OnComplete?.Invoke(this, result);
                OnAny?.Invoke(this, result);

                Logger.LogDebug($"Finished Worker {Name} {GetTime()} {GetThreadInfo()}");
            }
            catch (Exception e)
            {
                Timer.Stop();

                if (ContainsException<OperationCanceledException>(e))
                {
                    Logger.LogDebug($"Worker {Name} cancelled successfully {GetTime()} {GetThreadInfo()}");
                    return;
                }

                Fault = e;

                Status = DirectedTaskStatus.Faulted;

                result.TimeTaken = Timer.ElapsedMilliseconds;
                result.Fault = e;

                OnFault?.Invoke(this, result);
                OnAny?.Invoke(this, result);

                Logger.LogError($"Worker {Name} encountered an error {GetTime()} {GetThreadInfo()}");
                Logger.LogError(LogExceptions(e.InnerException));
            }
            finally
            {
                ReleaseSemaphore();

                OnFinal?.Invoke(this, result);
            }
        }

        public void Cancel()
        {
            // make sure we only cancel once
            if (Cancelled) return;
            Cancelled = true;

            // force the running task to stop
            TokenSource?.Cancel();

            // stop the timer so we're not needlessly running it
            Timer?.Stop();

            // release sempahore so other queued tasks can continue work
            ReleaseSemaphore();

            // create and notify subscribers
            var result = new TaskResult();
            result.Cancelled = true;
            result.TimeTaken = Timer.ElapsedMilliseconds;

            // set our status so it updates in UX
            Status = DirectedTaskStatus.Cancelled;

            // invoke callbacks
            OnCancel?.Invoke(this, result);
            OnAny?.Invoke(this, result);
            OnFinal?.Invoke(this, result);

            Logger.LogWarning($"Worker {Name} cancelled {GetTime()} {GetThreadInfo()}");
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

        public void Dispose()
        {
            // make sure we only dispose once
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(DirectedTask));
            }

            Disposed = true;

            Cancel();

            TokenSource?.Dispose();
        }

        public bool Equals(DirectedTask other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is DirectedTask task && task.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        private bool ContainsException<T>(Exception e) where T : Exception
        {
            if (e is T)
            {
                return true;
            }
            if (e.InnerException != null)
            {
                return ContainsException<T>(e.InnerException);
            }
            return false;
        }

        private string LogExceptions(Exception e)
        {
            string inner = e.InnerException != null ? LogExceptions(e.InnerException) : "";
            return $"<pre><div>{e.Message}</div><div>    {e.StackTrace}</div></pre>{inner}";
        }

        private string GetTime()
        {
            long time = Timer.ElapsedMilliseconds;

            StringBuilder postfix = new(1, 4);

            if (time < 1000)
            {
                postfix.Append("ms");
            }
            else if (time < 60_000)
            {
                time /= 1000;
                postfix.Append('s');
            }
            else if (time < 3600_000)
            {
                time /= 1000 / 60;
                postfix.Append("mins");
            }
            else if (time > 3600_000)
            {
                time /= 1000 / 60 / 60;
                postfix.Append("hrs");
            }

            return $"at Time {time.AsNumber()}{postfix.AsBold()}";
        }

        private string GetThreadInfo()
        {
            Thread current = Thread.CurrentThread;
            return $"on Thread {current.ManagedThreadId.AsNumber()}";
        }
    }
}
