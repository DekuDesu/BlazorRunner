using BlazorRunner.Runner.RuntimeHandling;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BlazorRunner.Runner
{
    public interface IScript : IBasicInfo, IDisposable, IInstanced
    {
        IInvokableMember Cleanup { get; set; }

        IInvokableMember EntryPoint { get; set; }

        IInvokableMember[] MiniScripts { get; set; }

        IInvokableMember Setup { get; set; }

        IScriptSetting[] Settings { get; set; }

        IDisposable ManagedResource { get; set; }
        IDictionary<string, IScriptSetting[]> SettingGroups { get; set; }
        IDictionary<string, IInvokableMember[]> ScriptGroups { get; set; }
        ILogger Logger { get; set; }

        object Invoke();
        object Invoke(CancellationToken token);
        Action<CancellationToken> ToAction();
    }
}