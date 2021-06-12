using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.RuntimeHandling
{
    public interface IScriptLogger : ILogger, IDisposable
    {
        IReadOnlyCollection<LogItem> Logs { get; }
        bool LowMemoryMode { get; set; }
        int MaxLogsKeptInMemory { get; set; }
        LogLevel MinimumLogLevel { get; set; }
        bool MirrorToConsole { get; set; }
        bool MirrorToFile { get; set; }
        string Path { get; set; }

        event Func<Task> OnLog;
    }
}