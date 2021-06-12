using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.RuntimeHandling
{
    public static class LoggerFactory
    {
        public static IScriptLogger CreateNewLogger()
        {
            return new DefaultLogger();
        }

        public static ILogger CreateLoggerSplitter(params ILogger[] LoggersToSplitTo)
        {
            return new LoggerSplitter(LoggersToSplitTo);
        }

        public static IScriptLogger CreateStreamLogger()
        {
            return new StreamLogger();
        }
    }
}
