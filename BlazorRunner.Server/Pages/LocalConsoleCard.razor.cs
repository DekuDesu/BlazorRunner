using BlazorRunner.Runner;
using BlazorRunner.Runner.RuntimeHandling;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorRunner.Server.Pages
{
    public partial class LocalConsoleCard : ComponentBase
    {
        [Parameter]
        public IScript Script { get; set; }

        private IScriptLogger Logger;

        private int SelectedTab = 0;

        private volatile Timer RefreshTimer = new(500);
        private object TimerLock = new();

        private IReadOnlyCollection<LogItem> All => Logger?.Logs;

        private IEnumerable<LogItem> Info => Logger?.Logs.Where(x => x.logLevel is LogLevel.Debug or LogLevel.Information or LogLevel.Trace);

        private IEnumerable<LogItem> Errors => Logger?.Logs.Where(x => x.logLevel is LogLevel.Error or LogLevel.Critical);

        private IEnumerable<LogItem> Warnings => Logger?.Logs.Where(x => x.logLevel is LogLevel.Warning);

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (Script != null)
                {
                    if (Script.Logger is LoggerSplitter splitter)
                    {
                        var logger = splitter.Loggers[1];
                        if (logger is IScriptLogger scriptLogger)
                        {
                            this.Logger = scriptLogger;
                        }
                    }
                }
                if (Logger != null)
                {
                    Logger.OnLog += Refresh;
                }

                lock (TimerLock)
                {
                    RefreshTimer.Elapsed += async (x, y) => await Refresh();
                    RefreshTimer.Start();
                }

            }
            await base.OnAfterRenderAsync(firstRender);
        }

        async Task Refresh()
        {
            await InvokeAsync(() => StateHasChanged());
        }

        public void Dispose()
        {
            lock (TimerLock)
            {
                RefreshTimer?.Stop();
                RefreshTimer?.Dispose();
                RefreshTimer = new(500);
                RefreshTimer.Enabled = true;
            }
        }

        public BootstrapColor GetColor(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => BootstrapColor.secondary,
                LogLevel.Debug => BootstrapColor.success,
                LogLevel.Information => BootstrapColor.none,
                LogLevel.Warning => BootstrapColor.warning,
                LogLevel.Error => BootstrapColor.danger,
                LogLevel.Critical => BootstrapColor.danger,
                LogLevel.None => BootstrapColor.none,
                _ => BootstrapColor.none,
            };
        }
    }
}
