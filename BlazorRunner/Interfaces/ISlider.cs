using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    /// <summary>
    /// Defines an object that has enough information to generate a slider at runtime
    /// </summary>
    public interface ISlider
    {
        /// <summary>
        /// Whether or not a slider is appropriate for the this type of object
        /// </summary>
        public bool SliderCompatible { get; set; }

        /// <summary>
        /// The maximum value this object can be set using the slider
        /// </summary>
        public object Max { get; set; }

        /// <summary>
        /// The minimum value this object can be set using the slider
        /// </summary>
        public object Min { get; set; }

        /// <summary>
        /// The value that each tick of the slider should be.
        /// </summary>
        public object StepSize { get; set; }
    }
}
