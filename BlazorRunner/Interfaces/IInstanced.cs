using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    /// <summary>
    /// Defines any <see cref="BlazorRunner"/> runtime type that contains a backing instance that provides a reference to the original
    /// object that was created using the loaded assembly
    /// </summary>
    public interface IInstanced
    {
        /// <summary>
        /// The instance that this object derives values from. 
        /// <para></para>
        /// Never assume this property is not <see langword="null"/>, for console applications or other entry-point based loaded asssemblies that do not use <see cref="BlazorRunner"/> Attributes these assemblies are not instanced and provided no backing instances or scripts.
        /// </summary>
        public object BackingInstance { get; init; }
    }
}
