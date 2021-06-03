using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public abstract class BaseAssemblyImporter : IAssemblyImporter
    {
        public LoadAssemblyResult Status { get; protected set; }

        public abstract Task<Assembly> LoadAsync();

        public abstract Assembly TryLoad();

        public virtual Assembly ParseBytes(byte[] bytes)
        {
            // make sure we don't try to continue if we failed to read the file
            if (bytes?.Length is null or 0)
            {
                return null;
            }

            // get the bytes of the assembly
            try
            {
                var assembly = Assembly.Load(bytes);

                Status = LoadAssemblyResult.Success;

                return assembly;
            }
            catch (ArgumentNullException)
            {
                Status = LoadAssemblyResult.NoBytesRead;
            }
            catch (BadImageFormatException)
            {
                Status = LoadAssemblyResult.BadImageFormat;
            }

            return null;
        }
    }
}
