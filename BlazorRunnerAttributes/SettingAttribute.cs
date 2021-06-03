using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Attributes
{
    /// <summary>
    /// Defines any property or field that should appear in the UX as a user-changable value, For example a <see langword="bool"/> field would appear as a toggle button, or a <see langword="double"/> property might appear as a slider
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SettingAttribute : System.Attribute
    {
        /// <summary>
        /// Allows you to combine settings into groups that will appear next to each other. This is NOT case sensitive
        /// </summary>
        public string Group { get; set; }
    }
}
