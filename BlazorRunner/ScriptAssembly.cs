using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public class ScriptAssembly : RegisteredObject, IScriptAssembly
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IScript[] Scripts => _Scripts;

        public IInvokableMember[] InvokableMembers => _InvokableMembers;

        public IScriptSetting[] Settings => _Settings;

        public ref IScript this[int index] => ref _Scripts[index];

        public object this[Guid Key] => GetUnknownObjectByKey(Key);

        public object this[string Key] => GetUnknownObjectByKey(new Guid(Key));

        public int Length => Scripts.Length;

        public IDictionary<Guid, IScript> ScriptDictionary { get; set; } = new Dictionary<Guid, IScript>();

        public IDictionary<Guid, IInvokableMember> InvokableDictionary { get; set; } = new Dictionary<Guid, IInvokableMember>();

        public IDictionary<Guid, IScriptSetting> SettingDictionary { get; set; } = new Dictionary<Guid, IScriptSetting>();

        /// <summary>
        /// <see langword="true"/> when this assembly is not a BlazorRunnerScript but a traditional DLL
        /// </summary>
        public bool IsGenericDLL { get; set; } = false;

        private IScript[] _Scripts = Array.Empty<IScript>();

        private IInvokableMember[] _InvokableMembers = Array.Empty<IInvokableMember>();

        private IScriptSetting[] _Settings = Array.Empty<IScriptSetting>();

        [DebuggerHidden]
        private object GetUnknownObjectByKey(Guid Key)
        {
            if (ScriptDictionary.ContainsKey(Key))
            {
                return ScriptDictionary[Key];
            }

            if (InvokableDictionary.ContainsKey(Key))
            {
                return InvokableDictionary[Key];
            }

            if (SettingDictionary.ContainsKey(Key))
            {
                return SettingDictionary[Key];
            }

            throw new KeyNotFoundException($"{Key} was not found as an instance of {nameof(IScript)}, {nameof(IInvokableMember)}, or {nameof(IScriptSetting)} in {Name} ({nameof(ScriptAssembly)}).");
        }

        public object InvokeMember(Guid ScriptId)
        {
            if (ScriptDictionary.ContainsKey(ScriptId))
            {
                return ScriptDictionary[ScriptId]?.Invoke();
            }

            if (InvokableDictionary.ContainsKey(ScriptId))
            {
                return InvokableDictionary[ScriptId]?.Invoke();
            }

            return null;
        }

        public void SetSetting(Guid SettingId, object Value)
        {
            if (SettingDictionary.ContainsKey(SettingId))
            {
                SettingDictionary[SettingId].Value = Value;
            }
        }

        public object GetSetting(Guid SettingId)
        {
            if (SettingDictionary.ContainsKey(SettingId))
            {
                return SettingDictionary[SettingId].Value;
            }

            return null;
        }

        public bool TryGetScript(Guid Id, out IScript found)
        {
            if (ScriptDictionary.ContainsKey(Id))
            {
                found = ScriptDictionary[Id];
                return true;
            }
            found = default;
            return false;
        }

        public void AddScript(IScript script)
        {
            if (ScriptDictionary.ContainsKey(script.Id) is false)
            {
                ScriptDictionary.Add(script.Id, script);
                lock (_Scripts)
                {
                    Helpers.Array.Append(ref _Scripts, script);
                }
            }

            AddInvokableMember(script.Setup);

            AddInvokableMember(script.EntryPoint);

            AddInvokableMember(script.Cleanup);

            foreach (var item in script.MiniScripts)
            {
                AddInvokableMember(item);
            }

            foreach (var item in script.Settings)
            {
                AddSetting(item);
            }
        }

        public IBasicInfo GetFlavorText(Guid Key)
        {
            object result = GetUnknownObjectByKey(Key);

            if (result is IBasicInfo info)
            {
                return info;
            }

            return null;
        }

        private void AddSetting(IScriptSetting setting)
        {
            if (setting != null)
            {
                if (SettingDictionary.ContainsKey(setting.Id) is false)
                {
                    SettingDictionary.Add(setting.Id, setting);
                    lock (_Settings)
                    {
                        Helpers.Array.Append(ref _Settings, setting);
                    }
                }
            }
        }

        private void AddInvokableMember(IInvokableMember member)
        {
            if (member != null)
            {
                if (InvokableDictionary.ContainsKey(member.Id) is false)
                {
                    InvokableDictionary.Add(member.Id, member);

                    lock (_InvokableMembers)
                    {
                        Helpers.Array.Append(ref _InvokableMembers, member);
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"{Name ?? Id.ToString()}";
        }

        public IEnumerator<IScript> GetEnumerator() => _Scripts.ToList().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _Scripts.GetEnumerator();

        public void Dispose()
        {
            // this is rather aggresive since we reference the backing instance of each script within all of the settings and 
            // invokables
            foreach (var item in _Scripts)
            {
                item?.Dispose();
            }

            InvokableDictionary.Clear();
            ScriptDictionary.Clear();
            SettingDictionary.Clear();

            _Scripts = null;
            _Settings = null;
            _InvokableMembers = null;
        }
    }
}
