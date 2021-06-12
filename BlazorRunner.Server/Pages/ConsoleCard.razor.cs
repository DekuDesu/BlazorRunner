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
    public partial class ConsoleCard : ComponentBase, IDisposable
    {
        private int SelectedTab = 0;

        private volatile Timer RefreshTimer = new(500);
        private object TimerLock = new();

        private IReadOnlyCollection<LogItem> All => LoggerDirector.GlobalLogger.Logs;

        private IEnumerable<LogItem> Info => LoggerDirector.GlobalLogger.Logs.Where(x => x.logLevel is LogLevel.Debug or LogLevel.Information or LogLevel.Trace);

        private IEnumerable<LogItem> Errors => LoggerDirector.GlobalLogger.Logs.Where(x => x.logLevel is LogLevel.Error or LogLevel.Critical);

        private IEnumerable<LogItem> Warnings => LoggerDirector.GlobalLogger.Logs.Where(x => x.logLevel is LogLevel.Warning);

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                LoggerDirector.GlobalLogger.OnLog += Refresh;

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
                LogLevel.Debug => BootstrapColor.primary,
                LogLevel.Information => BootstrapColor.info,
                LogLevel.Warning => BootstrapColor.warning,
                LogLevel.Error => BootstrapColor.danger,
                LogLevel.Critical => BootstrapColor.danger,
                LogLevel.None => BootstrapColor.none,
                _ => BootstrapColor.none,
            };
        }
    }
}
