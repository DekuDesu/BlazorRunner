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

        public Guid Id { get; init; }

        public Exception Fault { get; private set; } = null;

        public event Action<object, TaskResult> OnStart;
        public event Action<object, TaskResult> OnComplete;
        public event Action<object, TaskResult> OnFault;
        public event Action<object, TaskResult> OnCancel;
        public event Action<object, TaskResult> OnAny;



        public DirectedTask(Action Work, SemaphoreSlim Limiter, CancellationToken Token)
        {
            this.BackingAction = Work;

            this.Limiter = Limiter;

            this.BackingTask = new Task(Worker, Token);
        }

        private void Worker()
        {
            Limiter?.Wait();

            var result = new TaskResult();

            try
            {
                OnStart?.Invoke(this, result);
                OnAny?.Invoke(this, result);

                Timer.Start();

                BackingAction?.Invoke();

                Timer.Stop();

                result.TimeTaken = Timer.ElapsedMilliseconds;

                OnComplete?.Invoke(this, result);
                OnAny?.Invoke(this, result);
            }
            catch (OperationCanceledException)
            {
                Timer.Stop();

                result.TimeTaken = Timer.ElapsedMilliseconds;
                result.Cancelled = true;

                OnCancel?.Invoke(this, result);
                OnAny?.Invoke(this, result);
            }
            catch (Exception e)
            {
                Timer.Stop();

                Fault = e;

                result.TimeTaken = Timer.ElapsedMilliseconds;
                result.Fault = e;

                OnFault?.Invoke(this, result);
                OnAny?.Invoke(this, result);
            }
            finally
            {
                Limiter?.Release();
            }
        }
    }
}
