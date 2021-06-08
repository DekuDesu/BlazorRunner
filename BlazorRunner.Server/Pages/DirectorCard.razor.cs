using BlazorRunner.Runner;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorRunner.Server.Pages
{
    public partial class DirectorCard : ComponentBase, IDisposable
    {
        private volatile Timer RefreshTimer = new(500);
        private object TimerLock = new();
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                TaskDirector.OnTaskAny += UpdateAlt;

                lock (TimerLock)
                {
                    RefreshTimer.Elapsed += async (x, y) => await Refresh();
                    RefreshTimer.Start();
                }

            }
            await base.OnAfterRenderAsync(firstRender);
        }

        void UpdateAlt(Guid id)
        {
            InvokeAsync(() => StateHasChanged()).Wait();
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
    }
}
