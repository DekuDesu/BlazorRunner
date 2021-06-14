using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.RuntimeHandling
{
    public static class LoggerDirector
    {
        public static IScriptLogger GlobalLogger { get; set; } = LoggerFactory.CreateStreamLogger();

        public static IDictionary<Guid, ILogger> LoggerDictionary { get; set; } = new ConcurrentCallbackDictionary<Guid, ILogger>();

        public static string LoggingDirectory { get; set; } = "";

        public static string LogFilePrefix { get; set; } = "Prefix";

        public static string LogFilePostfix { get; set; } = "PostFix";

        public static string LogFileExtension { get; set; } = ".log";

        public static bool LowMemoryMode { get; set; } = false;

        public static int MaxLogsKeptInMemory { get; set; } = 1_000;

        public static LogLevel MinimumLogLevel { get; set; } = LogLevel.Trace;

        public static bool MirrorToFile { get; set; } = false;

        public static bool MirrorToConsole { get; set; } = false;

        public static ILogger CreateLogger(Guid Id, string FileName = null)
        {
            // create a logger for the script
            ILogger newLogger = LoggerFactory.CreateNewLogger();

            // this needs to come first since assigning some properties to DefaultLogger
            // uses Path, this isn't ideal.
            AssignPathInformation(newLogger, FileName);

            AssignDefaultProperties(newLogger);

            // create a splitter so when the script logs it logs to the global and to it's local console
            ILogger splitter = LoggerFactory.CreateLoggerSplitter(GlobalLogger, newLogger);

            // add the pair to the dict so just incase we need it, we can access it later
            LoggerDictionary.TryAdd(Id, newLogger);

            return splitter;
        }

        public static void RedirectConsole()
        {
            if (GlobalLogger is StreamLogger logger)
            {
                logger.OutWriter = Console.Out;
                logger.MinimumLogLevel = MinimumLogLevel;
                logger.MirrorToFile = true;
                logger.MirrorToConsole = false;
                Console.SetOut(logger);
                Console.SetError(logger);
            }
        }

        private static void AssignPathInformation(ILogger logger, string FileName)
        {
            FileName ??= Guid.NewGuid().ToString()[1..^2];

            if (logger is DefaultLogger defaultLogger)
            {
                defaultLogger.Path = Path.Combine(LoggingDirectory, string.Join("", LogFilePrefix, FileName, LogFilePostfix, LogFileExtension));
            }
        }

        private static void AssignDefaultProperties(ILogger logger)
        {
            if (logger is DefaultLogger defaultLogger)
            {
                defaultLogger.MinimumLogLevel = MinimumLogLevel;
                defaultLogger.MirrorToFile = MirrorToFile;
                defaultLogger.MirrorToConsole = MirrorToConsole;
                defaultLogger.LowMemoryMode = LowMemoryMode;
                defaultLogger.MaxLogsKeptInMemory = MaxLogsKeptInMemory;
            }
        }

        public static void FlushAll()
        {
            var values = LoggerDictionary.Values;

            foreach (var item in values)
            {
                if (item is IScriptLogger logger)
                {
                    logger.Flush();
                }
            }

            LoggerDictionary.Clear();

            GlobalLogger.Flush();
        }
    }
}
