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
            throw new NotImplementedException();
        }

        private string FormatForLogFile<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            return $"{logLevel},{}{}{}";
        }

        private ref struct LogObject<TState>
        {
            public LogLevel logLevel { get; set; }

            public EventId eventId { get; set; }

            public TState state { get; set; }

            public Exception exception { get; set; }

            public string FormattedString { get; set; }

            public override string ToString()
            {
                return base.ToString();
            }
        }
    }
}
