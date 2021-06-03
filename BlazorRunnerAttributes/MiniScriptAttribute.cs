using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Attributes
{
    /// <summary>
    /// Any method with this attached will appear as a seperate piece of standalone executable code in addition to the script this is attached to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MiniScriptAttribute : System.Attribute
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Group { get; set; }
    }
}
