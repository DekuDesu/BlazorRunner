using BlazorRunner.Runner.RuntimeHandling;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public class Script : RegisteredObject, IScript
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public object BackingInstance { get; init; }

        public IInvokableMember Setup { get; set; }

        public IInvokableMember EntryPoint { get; set; }

        public IInvokableMember Cleanup { get; set; }

        public IInvokableMember[] MiniScripts { get; set; } = Array.Empty<IInvokableMember>();

        public IScriptSetting[] Settings { get; set; } = Array.Empty<IScriptSetting>();

        public IDictionary<string, IScriptSetting[]> SettingGroups { get; set; }

        public IDictionary<string, IInvokableMember[]> ScriptGroups { get; set; }

        public ILogger Logger { get; set; }

        public IDisposable ManagedResource { get; set; }


        public object Invoke()
        {
            Setup?.Invoke();

            object result = EntryPoint?.Invoke();

            Cleanup?.Invoke();

            return result;
        }

        public override string ToString()
        {
            return $"{Name ?? Id.ToString()}";
        }

        public virtual void Dispose()
        {
            ManagedResource?.Dispose();
        }
    }
}
