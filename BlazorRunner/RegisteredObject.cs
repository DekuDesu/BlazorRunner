using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    /// <summary>
    /// Defines the base <see langword="object"/> that all <see cref="BlazorRunner"/> runtime types derive. The only purpose of this type
    /// Is to define that this type has a <see langword="readonly"/> <see cref="Guid"/> Property that is gaurunteed to be unique. Should the
    /// <see cref="Guid"/> not be unique a warning will be thrown. Default is <see langword="10_000"/> attempts to create unique id for the
    /// runtime type.
    /// </summary>
    public abstract class RegisteredObject
    {
        /// <summary>
        /// The registered Id of this object. Will always be unique, if not an warning is thrown.
        /// </summary>
        public virtual Guid Id { get; } = GuidRegistrar.Create();
    }
}
