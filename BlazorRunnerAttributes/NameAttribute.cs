using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Attributes
{
    /// <summary>
    /// Allows you to define the name that should be displayed in the UX for <see cref="BlazorRunner"/>, the default(no attribute) is just the object's signature.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public class NameAttribute : System.Attribute
    {
        /// <summary>
        /// The name that should be displayed in the UX for <see cref="BlazorRunner"/>, the default(no attribute) is just the object's signature.
        /// </summary>
        public string Name { get; set; }

        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}
