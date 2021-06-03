using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Attributes
{
    /// <summary>
    /// This is ran every time before any method with the <see cref="EntryPointAttribute"/> is executed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SetupAttribute : System.Attribute
    {
        /// <summary>
        /// Whether or not the method that this attribute is attached to should run only once(<see langword="true"/>) or always(<see langword="false"/>)
        /// </summary>
        public bool RunOnce { get; set; } = false;

        /// <summary>
        /// Whether or not this method has ran before
        /// </summary>
        public bool Ran { get; set; } = false;
    }
}
