using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.RuntimeHandling
{
    /// <summary>
    /// Redirects and splits logs sent to this <see cref="ILogger"/> to all <see cref="ILogger"/>s provided to it's constructor.
    /// </summary>
    public class LoggerSplitter : ILogger
    {
        private readonly ILogger[] Loggers = Array.Empty<ILogger>();

        public LoggerSplitter(params ILogger[] loggers)
        {
            Loggers = loggers;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            foreach (var item in Loggers)
            {
                if (!item.IsEnabled(logLevel))
                {
                    return false;
                }
            }
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            foreach (var item in Loggers)
            {
                item.Log(logLevel, eventId, state, exception, formatter);
            }
        }
    }
}
