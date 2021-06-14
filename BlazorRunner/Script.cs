using BlazorRunner.Runner.RuntimeHandling;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public int Count => GetInvokableMemberCount();

        public ILogger Logger { get; set; }

        public IDisposable ManagedResource { get; set; }

        public bool IsGenericDLL { get; set; } = false;

        private int GetInvokableMemberCount()
        {
            int count = 0;
            if (Setup != null)
            {
                count++;
            }
            if (EntryPoint != null)
            {
                count++;
            }
            if (Cleanup != null)
            {
                count++;
            }
            count += MiniScripts.Length;
            return count;
        }

        public Action<CancellationToken> ToAction()
        {
            return (CancellationToken token) => Invoke(token);
        }

        public object Invoke(CancellationToken token)
        {
            Setup?.Invoke(token);

            object result = EntryPoint?.Invoke(token);

            Cleanup?.Invoke(token);

            return result;
        }

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
