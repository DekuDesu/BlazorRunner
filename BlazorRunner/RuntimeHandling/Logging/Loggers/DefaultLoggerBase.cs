using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.RuntimeHandling
{
    public abstract class DefaultLoggerBase : IScriptLogger
    {

        private readonly object LogLock = new();

        internal List<LogItem> _Logs = new();

        private bool _MirrorToFile = false;

        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Trace;

        public string Path { get; set; } = "";

        public TextWriter OutWriter { get; set; }

        public IReadOnlyCollection<LogItem> Logs => VolatileCopy();

        public bool MirrorToConsole { get; set; } = false;

        public bool MirrorToFile
        {
            get => _MirrorToFile;
            set
            {
                if (value)
                {
                    OutWriter ??= new StreamWriter(Path);
                }
                _MirrorToFile = value;
            }
        }

        public int MaxLogsKeptInMemory { get; set; } = 1_000;

        public bool LowMemoryMode { get; set; } = false;

        public object SynchronizationObject => LogLock;

        public event Func<Task> OnLog;


        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public abstract void Dispose();

        public abstract bool IsEnabled(LogLevel logLevel);

        public abstract void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);

        protected abstract void AddLog(LogItem item);

        [DebuggerHidden]
        protected LogItem[] VolatileCopy()
        {
            LogItem[] logs = null;

            lock (SynchronizationObject)
            {
                logs = new LogItem[_Logs.Count];
                _Logs.CopyTo(logs);
            }

            return logs.Reverse().ToArray();
        }

        protected void InvokeOnLog()
        {
            if (OnLog != null)
            {
                Task.Run(OnLog);
            }
        }

        public virtual void Flush()
        {
            OutWriter?.Flush();
            lock (SynchronizationObject)
            {
                _Logs.Clear();
            }
        }
    }
}