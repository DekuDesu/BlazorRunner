﻿using System;
using System.Collections.Generic;

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

        object Invoke();
    }
}