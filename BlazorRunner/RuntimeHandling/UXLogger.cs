using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BlazorRunner.Runner.RuntimeHandling
{
    public class UXLogger : ILogger
    {
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Debug;

        public bool LogToFile { get; set; } = false;

        public string Path { get; init; } = $"BlazorRunnerLog_{Guid.NewGuid()}.txt";

        public IReadOnlyCollection<string> Logs => _Logs;

        internal List<string> _Logs = new();
        private readonly object LogLock = new();

        public UXLogger()
        {

        }

        public UXLogger(string Path)
        {
            this.Path = Path;
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
            string log = FormatForLogFile(logLevel, eventId, state, exception, formatter);

            lock (LogLock)
            {
                _Logs.Add(log);
            }

            Console.WriteLine($"Logged {log}");
        }

        private static string FormatForLogFile<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            return $"{logLevel},{eventId},{state},{exception},{formatter(state, exception)}";
        }
    }
}
