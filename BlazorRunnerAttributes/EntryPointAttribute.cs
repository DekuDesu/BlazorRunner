using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Attributes
{
    /// <summary>
    /// This method should represent the main functionality of a class with a <see cref="ScriptAttribute"/>. It's optional.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class EntryPointAttribute : System.Attribute
    {
    }
}
