using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace BlazorRunner.Runner.RuntimeHandling
{
    public class StreamLogger : TextWriter, IScriptLogger
    {
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Debug;

        [NotNull]
        public string Path { get; set; } = "";

        [MaybeNull]
        public TextWriter OutWriter { get; set; }

        public IReadOnlyCollection<LogItem> Logs => _Logs;

        public bool MirrorToConsole { get; set; } = false;

        public bool MirrorToFile { get; set; } = false;

        public int MaxLogsKeptInMemory { get; set; } = 1_000;

        public bool LowMemoryMode { get; set; } = false;

        [NotNull]
        public override Encoding Encoding { get; } = Encoding.UTF8;

        internal List<LogItem> _Logs = new();

        private readonly object LogLock = new();

        public StreamLogger()
        {
            OnLog = default!;
        }

        public StreamLogger(string Path)
        {
            this.Path = Path;
            this.MirrorToFile = true;
            OnLog = default!;
        }

        public StreamLogger(IFormatProvider formatProvider) : base(formatProvider)
        {
            OnLog = default!;
        }

        public event Func<Task> OnLog;


        private void LogValue([AllowNull] object value)
        {
            string s = value?.ToString() ?? "";

            var newLog = new LogItem(LogLevel.Information, new EventId(), value, null, s);

            AddLog(newLog);

            if (OnLog != null)
            {
                Task.Run(OnLog);
            }

            if (MirrorToFile)
            {
                OutWriter?.WriteLine(newLog.ToString());
            }
        }

        private void LogEnumerable<T>(IEnumerable<T>? enumerable)
        {
            if (enumerable != null)
            {
                LogValue(string.Join("", enumerable));
                return;
            }
            LogValue(null);
        }

        private void LogFormattedString(string format, params object?[]? parameters)
        {
            if (format != null && parameters != null)
            {
                LogValue(string.Format(format!, parameters));
            }
            else
            {
                if (parameters is null)
                {
                    LogValue(format);
                }

                LogValue(null);
            }

        }

        public override void WriteLine(object? value) => LogValue(value);

        public override void WriteLine() => LogValue(null);

        public override void WriteLine(bool value) => LogValue(value);

        public override void WriteLine(char value) => LogValue(value);

        public override void WriteLine(char[]? buffer) => LogEnumerable(buffer);

        public override void WriteLine(char[] buffer, int index, int count) => LogEnumerable(buffer[index..(index + count)]);

        public override void WriteLine(decimal value) => LogValue(value);

        public override void WriteLine(double value) => LogValue(value);

        public override void WriteLine(float value) => LogValue(value);

        public override void WriteLine(int value) => LogValue(value);

        public override void WriteLine(uint value) => LogValue(value);

        public override void WriteLine(long value) => LogValue(value);

        public override void WriteLine(ulong value) => LogValue(value);

        public override void WriteLine(string? value) => LogValue(value);

        public override void WriteLine(string format, object? arg0) => LogFormattedString(format, arg0!);

        public override void WriteLine(string format, object? arg0, object? arg1) => LogFormattedString(format, arg0!, arg1!);

        public override void WriteLine(string format, object? arg0, object? arg1, object? arg2) => LogFormattedString(format, arg0!, arg1!, arg2!);

        public override void WriteLine(string format, params object?[]? arg)
        {
            if (arg == null)                       // avoid ArgumentNullException from String.Format
                LogFormattedString(format, null, null); // faster than base.WriteLine(format, (Object)arg);
            else
                LogFormattedString(format, arg);
        }

        public override void Write(string format, object? arg0) => LogFormattedString(format, arg0);

        public override void Write(string format, object? arg0, object? arg1) => LogFormattedString(format, arg0, arg1);

        public override void Write(string format, object? arg0, object? arg1, object? arg2) => LogFormattedString(format, arg0, arg1, arg2);

        public override void Write(string format, params object?[]? arg)
        {
            if (arg == null)                   // avoid ArgumentNullException from String.Format
                LogFormattedString(format, null, null); // faster than base.Write(format, (Object)arg);
            else
                LogFormattedString(format, arg);
        }

        public override void Write(bool value) => LogValue(value);

        public override void Write(char value) => LogValue(value);

        public override void Write(char[]? buffer) => LogEnumerable(buffer);

        public override void Write(char[] buffer, int index, int count) => LogEnumerable(buffer[index..(index + count)]);

        public override void Write(double value) => LogValue(value);

        public override void Write(decimal value) => LogValue(value);

        public override void Write(float value) => LogValue(value);

        public override void Write(int value) => LogValue(value);

        public override void Write(uint value) => LogValue(value);

        public override void Write(long value) => LogValue(value);

        public override void Write(ulong value) => LogValue(value);

        public override void Write(object? value) => LogValue(value);

        public override void Write(string? value) => LogValue(value);

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

        public new void Dispose()
        {
            OutWriter?.Dispose();
            base.Dispose();
        }

        ~StreamLogger()
        {
            Dispose();
        }
    }
}
