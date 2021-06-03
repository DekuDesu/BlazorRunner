using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RangeAttribute : System.Attribute
    {
        /// <summary>
        /// Set a minimum for this setting to be set before runtime
        /// </summary>
        public object Min { get; set; }

        /// <summary>
        /// Set a maximum for this setting to be before runtime
        /// </summary>
        public object Max { get; set; }

        /// <summary>
        /// If the setting this attribute is applied to supports sliders(like primitive numbers) this adjusts the space between each
        /// tick of the slider, default is a small number depending on primitive
        /// </summary>
        public object StepAmount { get; set; }
    }
}
