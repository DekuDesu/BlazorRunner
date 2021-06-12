using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BlazorRunner.Runner.RuntimeHandling
{
    public class DefaultLogger : IScriptLogger
    {
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Debug;

        public string Path { get; set; } = "";

        public TextWriter OutWriter { get; set; }

        public IReadOnlyCollection<LogItem> Logs => _Logs;

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
        private bool _MirrorToFile = false;

        public int MaxLogsKeptInMemory { get; set; } = 1_000;

        public bool LowMemoryMode { get; set; } = false;

        internal List<LogItem> _Logs = new();

        private readonly object LogLock = new();

        public event Func<Task> OnLog;

        public DefaultLogger()
        {

        }

        public DefaultLogger(string Path)
        {
            this.Path = Path;
            this.OutWriter = new StreamWriter(Path);
            this.MirrorToFile = true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= MinimumLogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (logLevel < MinimumLogLevel)
            {
                return;
            }

            LogItem newItem = new(logLevel, eventId, state, exception, formatter(state, exception));

            AddLog(newItem);

            if (OnLog != null)
            {
                Task.Run(OnLog);
            }

            if (MirrorToFile)
            {
                OutWriter?.WriteLine(newItem.ToString());
            }

            if (MirrorToConsole)
            {
                Console.WriteLine(newItem);
            }
        }

        private void AddLog(LogItem item)
        {
            if (LowMemoryMode)
            {
                if (_Logs.Count >= MaxLogsKeptInMemory)
                {
                    lock (LogLock)
                    {
                        _Logs.RemoveAt(0);
                        _Logs.Add(item);
                    }
                    return;
                }
            }

            lock (LogLock)
            {
                _Logs.Add(item);
            }
        }

        public void Dispose()
        {
            OutWriter?.Dispose();
        }

        ~DefaultLogger()
        {
            Dispose();
        }
    }
}
