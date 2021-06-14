using BlazorRunner.Runner;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorRunner.Server.Pages
{
    public partial class ImportCard : ComponentBase
    {
        private int SelectedTab = 0;

        //private volatile Timer RefreshTimer = new(500);
        //private readonly object TimerLock = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //lock (TimerLock)
                //{
                //    RefreshTimer.Elapsed += async (x, y) => await Refresh();
                //    RefreshTimer.Start();
                //}

            }
            await base.OnAfterRenderAsync(firstRender);
        }


        private void LoadFiles(InputFileChangeEventArgs e)
        {
            Console.WriteLine("Did something");
        }

        async Task Refresh()
        {
            await InvokeAsync(() => StateHasChanged());
        }

        public void Dispose()
        {
            //lock (TimerLock)
            //{
            //    RefreshTimer?.Stop();
            //    RefreshTimer?.Dispose();
            //    RefreshTimer = new(500);
            //    RefreshTimer.Enabled = true;
            //}
        }
    }
}
