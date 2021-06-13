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

        private bool Running = false;

        public async Task InvokeMember()
        {
            Running = true;

            if (Index.SelectedAssembly.TryGetScript(Member.Parent, out var Parent))
            {
                for (int i = 0; i < 1; i++)
                {
                    TaskDirector.QueueTask(Member.ToAction(), Member.Id, Parent.Logger);
                }
            }

            await Task.Delay(250);

            Running = false;
        }
    }
}
