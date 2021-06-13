using BlazorRunner.Runner;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class ScriptCard : ComponentBase
    {
        [Parameter]
        public IScript Script { get; set; }

        private async Task QueueEntryPoint()
        {
            await TaskDirector.QueueTask(Script.ToAction(), Script.EntryPoint.Id, Script.Logger, Script.Name);
        }
    }
}
