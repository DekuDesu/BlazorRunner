﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public static class Factory
    {
        public static IScriptAssembly CreateScriptAssembly()
        {
            return new ScriptAssembly();
        }

        public static IScript CreateScript(object BackingInstance)
        {
            return new Script() { BackingInstance = BackingInstance };
        }

        public static IInvokableMember CreateInvokableMember(MethodInfo method, object BackingInstance)
        {
            return new InvokableMember() { BackingInstance = BackingInstance, BackingMethod = method };
        }

        public static IScriptSetting CreateScriptSetting(PropertyInfo propertyInfo, object BackingInstance)
        {
            return new ScriptSetting() { BackingProperty = propertyInfo, BackingInstance = BackingInstance };
        }

        public static IScriptSetting CreateScriptSetting(FieldInfo fieldInfo, object BackingInstance)
        {
            return new ScriptSetting() { BackingField = fieldInfo, BackingInstance = BackingInstance };
        }
    }
}
