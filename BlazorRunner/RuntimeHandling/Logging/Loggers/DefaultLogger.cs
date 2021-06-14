using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BlazorRunner.Runner.RuntimeHandling
{

    public class DefaultLogger : DefaultLoggerBase
    {
        [DebuggerHidden]
        public override bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= MinimumLogLevel && logLevel != LogLevel.None;
        }

        [DebuggerHidden]
        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel) is false)
            {
                return;
            }

            LogItem newItem = new(logLevel, eventId, state, exception, formatter(state, exception));

            AddLog(newItem);

            InvokeOnLog();

            if (MirrorToFile)
            {
                OutWriter?.WriteLine(newItem.ToString());
            }

            if (MirrorToConsole)
            {
                Console.WriteLine(newItem);
            }
        }

        [DebuggerHidden]
        protected override void AddLog(LogItem item)
        {
            if (LowMemoryMode)
            {
                if (_Logs.Count >= MaxLogsKeptInMemory)
                {
                    lock (base.SynchronizationObject)
                    {
                        _Logs.RemoveAt(0);
                        _Logs.Add(item);
                    }
                    return;
                }
            }

            lock (base.SynchronizationObject)
            {
                _Logs.Add(item);
            }
        }

        [DebuggerHidden]
        public override void Dispose()
        {
            OutWriter?.Dispose();
        }

        ~DefaultLogger()
        {
            Dispose();
        }
    }
}
