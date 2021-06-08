using System;
using System.Collections.Generic;

namespace BlazorRunner.Runner
{
    public interface IScriptAssembly : IBasicInfo, IEnumerable<IScript>, IDisposable
    {
        /// <summary>
        /// Retrieve any <see cref="IScript"/>, <see cref="IInvokableMember"/>, or <see cref="IScriptSetting"/> using it's <see cref="Guid"/>
        /// <code>
        /// Complexity: O(1)
        /// </code>
        /// </summary>
        /// <param name="Key">The <see cref="RegisteredObject.Id"/> of the object to be found in the <see cref="IScriptAssembly"/></param> 
        /// <returns></returns>
        object this[Guid Key] { get; }

        /// <summary>
        /// Retrieve the <see cref="IScript"/> using the index of it in the <see cref="IScriptAssembly.Scripts"/> array
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        ref IScript this[int index] { get; }

        /// <summary>
        /// The loaded scripts  within this <see cref="IScriptAssembly"/>
        /// </summary>
        IScript[] Scripts { get; }

        /// <summary>
        /// The <see cref="IInvokableMember"/>s from all scripts within this <see cref="IScriptAssembly"/>
        /// </summary>
        IInvokableMember[] InvokableMembers { get; }

        /// <summary>
        /// The <see cref="IScriptSetting"/>s from all scripts within this <see cref="IScriptAssembly"/>
        /// </summary>
        IScriptSetting[] Settings { get; }

        /// <summary>
        /// The number of <see cref="IScript"/>s loaded into this <see cref="IScriptAssembly"/>
        /// </summary>
        int Length { get; }

        void AddScript(IScript script);
        IBasicInfo GetFlavorText(Guid Key);
        object GetSetting(Guid SettingId);

        object InvokeMember(Guid ScriptId);

        void SetSetting(Guid SettingId, object Value);
    }
}