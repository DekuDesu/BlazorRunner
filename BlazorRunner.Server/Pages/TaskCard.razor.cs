using BlazorRunner.Runner;
using BlazorRunner.Runner.RuntimeHandling;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorRunner.Server.Pages
{
    public partial class TaskCard : ComponentBase
    {
        [Parameter]
        public DirectedTask Task { get; set; }

        private IBasicInfo RetrievedInfo = null;

        private string Name => RetrievedInfo?.Name ?? "Loading";

        private string Description => RetrievedInfo?.Description ?? "Loading";

        [Parameter]
        public DirectedTaskStatus Status { get; set; }

        [Parameter]
        public long Time { get; set; }

        private bool showQuestion = false;

        async Task ShowEndTask()
        {
            showQuestion = true;
            await System.Threading.Tasks.Task.Delay(1);
        }

        async Task HideEndTask()
        {
            showQuestion = false;
            await System.Threading.Tasks.Task.Delay(1);
        }

        async Task EndTask()
        {
            Task?.Cancel();
            await System.Threading.Tasks.Task.Delay(1);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (Task != null)
            {
#pragma warning disable CS0234
                RetrievedInfo = GetFlavorText(Task.BackingId);
#pragma warning restore CS0234
            }
        }

        private IBasicInfo GetFlavorText(Guid id)
        {
            if (BlazorRunner.Server.Pages.Index.SelectedAssembly != null)
            {
                return BlazorRunner.Server.Pages.Index.SelectedAssembly.GetFlavorText(id);
            }
            else
            {
                var assemblies = AssemblyDirector.GetAssemblies();

                for (int i = 0; i < assemblies.Length; i++)
                {
                    var info = assemblies[i].GetFlavorText(id);
                    if (info != null)
                    {
                        return info;
                    }
                }
            }
            return null;
        }
    }
}
