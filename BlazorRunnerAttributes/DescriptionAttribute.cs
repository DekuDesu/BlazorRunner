using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Attributes
{
    /// <summary>
    /// Allows you to define the description that should be displayed in the UX of <see cref="BlazorRunner"/>, the default is none
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public class DescriptionAttribute : System.Attribute
    {
        /// <summary>
        /// The description that should be displayed in the UX of <see cref="BlazorRunner"/>, the default is none
        /// </summary>
        public string Description { get; set; }

        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
