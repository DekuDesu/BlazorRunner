using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.Helpers
{
    public static class Exceptions
    {
        [DebuggerHidden]
        public static Exception NoParameterlessConstructor(Type type)
        {
            throw new NotSupportedException($"Failed to create a script using the type {type.Name} becuase it does not have a public parameterless constructor.");
        }

        [DebuggerHidden]
        public static Exception IncompatibleWithGenericClasses(Type type)
        {
            throw new NotSupportedException($"Failed to create a script for {type.FullName} becuase it is a generic class. These are not supported at this time.");
        }

        [DebuggerHidden]
        public static Exception IncompatibleWithGenericMethods(MethodInfo method)
        {
            throw new NotSupportedException($"Failed to create a script for {method.Name}, becuase {method.Name} contains generic parameters. These are not supported at this time.");
        }

        [DebuggerHidden]
        public static Exception IncompatibleWithParameters(MethodInfo method)
        {
            return new NotSupportedException($"Failed to create a script for {method.Name}, becuase {method.Name} contains parameters. These are not supported at this time. Consider using SettingAttributes in conjunction with properties/fields in the script to use as runtime parameters.");
        }

        [DebuggerHidden]
        public static Exception IncompatibleWithGenericSettings(MemberInfo info)
        {
            return new NotSupportedException($"Failed to create a setting for {info.Name} becuase it is a generic field and/or property. This isn't supported at this time.");
        }

        [DebuggerHidden]
        public static Exception IncompatibleEntryPoint(MethodInfo info)
        {
            return new NotSupportedException($"Failed to create a script from the entry point {info.Name}. The entry point must comply with standard entry point signatures see: https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/program-structure/main-command-line");
        }

        [DebuggerHidden]
        public static Exception IncompatibleTypeUsedWithRange(Type Expected, Type Actual)
        {
            return new InvalidOperationException($"Failed to cast {Actual.Name} to the expected type of {Expected.Name}. When using the [Range(Min, Max, Step)] attribute, ensure that the types of Min, Max and Step are of the same type, or converitble to the [Setting] type.");
        }
    }
}
