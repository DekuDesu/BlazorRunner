using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.Helpers
{
    public static class Warnings
    {
        [DebuggerHidden]
        public static string PushWarning(string warning)
        {
            Console.Error.WriteLine(warning);

            return warning;
        }

        [DebuggerHidden]
        public static string MultipleMethodsWithAttribute(Type AttributeType, string ScriptName = "")
        {
            return PushWarning($"There are multilple methods with the {AttributeType.Name} in {ScriptName}. Usually there is only one of them. Only the first one will be invoked instead of all of them.");
        }

        public static string TooManyGuidsGenerated(Guid id, int NumberOfCurrentGuids, int WarningThreashold)
        {
            return PushWarning($"Failed to generate unique Guid to identify a new object. Guid {id} is a duplicate Id and may cause issues with dynamically modifying/invoking scripts and values associated with it and it's twin Id. There are {NumberOfCurrentGuids} Guids in circulation. Attempted to generate a unique Guid {WarningThreashold} times but failed. Consider restarting the application to free un-used Guids.");
        }
    }
}
