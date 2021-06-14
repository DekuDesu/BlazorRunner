using BlazorRunner.Runner;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class InvokableMemberControl : ComponentBase
    {
        [Parameter]
        public IInvokableMember Member { get; set; }

        DirectedTask StoredTask;

        DirectedTaskStatus Status => StoredTask?.Status ?? DirectedTaskStatus.none;

        private bool Running = false;

        public async Task InvokeMember()
        {
            Running = true;

            if (Index.SelectedAssembly.TryGetScript(Member.Parent, out var Parent))
            {
                StoredTask = await TaskDirector.QueueTask(Member.ToAction(), Member.Id, Parent.Logger, Member.Name);

                StoredTask.OnAny += (x, y) => { InvokeAsync(StateHasChanged).Wait(); };
            }

            await Task.Delay(250);

            Running = false;
        }
    }
}
