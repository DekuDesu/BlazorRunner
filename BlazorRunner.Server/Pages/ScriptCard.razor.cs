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

        DirectedTaskStatus Status => StoredTask?.Status ?? DirectedTaskStatus.none;

        DirectedTask StoredTask;

        private string[] StringArgs = Array.Empty<string>();

        private void UpdateStringArgs(object val)
        {
            // "path" arg -g -g up "helper" "path"

            StringArgs = GetArgs(val.ToString());
        }

        private string[] GetArgs(string str)
        {
            // converts
            // "path" arg -g -g up "helper" "path"
            // to string[]
            // path, arg, -g, -g, up, helper, path
            List<string> vals = new();

            var split = str.Split("\"").Where(x => String.IsNullOrWhiteSpace(x) is false);

            foreach (var item in split)
            {
                string[] subsplit = item.Split(" ");
                if (subsplit.Length is 0 or 1)
                {
                    vals.Add(item);
                    continue;
                }
                foreach (var subitem in subsplit)
                {
                    vals.Add(subitem);
                }
            }

            return vals.Where(x => String.IsNullOrWhiteSpace(x) is false)?.ToArray() ?? Array.Empty<string>();
        }

        private async Task QueueEntryPoint()
        {
            if (Script.IsGenericDLL && Script.EntryPoint.DefaultParameters != null)
            {
                Script.EntryPoint.DefaultParameters = StringArgs;

            }

            StoredTask = await TaskDirector.QueueTask(Script.ToAction(), Script.EntryPoint.Id, Script.Logger, Script.Name);

            StoredTask.OnAny += (x, y) => { InvokeAsync(StateHasChanged).Wait(); };

            await Task.Delay(250);
        }
    }
}
