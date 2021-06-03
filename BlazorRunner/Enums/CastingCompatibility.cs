using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    /// <summary>
    /// Defines the compatibility between an instance and a destination type
    /// </summary>
    public enum CastingCompatibility
    {
        /// <summary>
        /// There was no conversion found
        /// </summary>
        none,
        /// <summary>
        /// The type was the same type as the destination type
        /// </summary>
        SameType,
        /// <summary>
        /// The type was implicitly convertibe to the destination type
        /// </summary>
        Implicit,
        /// <summary>
        /// The type can be converted explicitly using a cast
        /// </summary>
        Explicit,
        /// <summary>
        /// The type can be converted to the destination type using string parsing
        /// </summary>
        Parsable
    }
}