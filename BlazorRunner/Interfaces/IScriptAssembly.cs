using System;
using System.Collections.Generic;

namespace BlazorRunner.Runner
{
    public interface IScriptAssembly : IBasicInfo, IEnumerable<IScript>, IDisposable
    {
        object this[Guid Key] { get; }

        ref IScript this[int index] { get; }

        IScript[] Scripts { get; }
        IInvokableMember[] InvokableMembers { get; }
        IScriptSetting[] Settings { get; }

        void AddScript(IScript script);
        object GetSetting(Guid SettingId);
        object InvokeMember(Guid ScriptId);
        void SetSetting(Guid SettingId, object Value);
    }
}