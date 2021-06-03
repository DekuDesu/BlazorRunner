using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    /// <summary>
    /// Imports assemblies from local files using the provided path
    /// </summary>
    public class AssemblyReaderImporter : BaseAssemblyImporter
    {
        public string Path { get; set; }

        public AssemblyReaderImporter(string Path)
        {
            this.Path = Path;
        }

        public override Assembly TryLoad()
        {
            byte[] rawBytes = null;

            // attempt to read the file
            try
            {
                rawBytes = File.ReadAllBytes(Path);
            }
            catch (ArgumentNullException)
            {
                Status = LoadAssemblyResult.PathWasNull;
            }
            catch (ArgumentException)
            {
                Status = LoadAssemblyResult.PathWasInvalid;
            }
            catch (PathTooLongException)
            {
                Status = LoadAssemblyResult.PathWasTooLong;
            }
            catch (DirectoryNotFoundException)
            {
                Status = LoadAssemblyResult.DirectoryNotFound;
            }
            catch (IOException)
            {
                Status = LoadAssemblyResult.FailedToGetFileHandle;
            }
            catch (UnauthorizedAccessException)
            {
                Status = LoadAssemblyResult.UnauthorizedAccess;
            }

            return base.ParseBytes(rawBytes);
        }

        public override async Task<Assembly> LoadAsync()
        {
            byte[] rawBytes = null;

            // attempt to read the file
            try
            {
                rawBytes = await File.ReadAllBytesAsync(Path);
            }
            catch (ArgumentNullException)
            {
                Status = LoadAssemblyResult.PathWasNull;
            }
            catch (ArgumentException)
            {
                Status = LoadAssemblyResult.PathWasInvalid;
            }
            catch (PathTooLongException)
            {
                Status = LoadAssemblyResult.PathWasTooLong;
            }
            catch (DirectoryNotFoundException)
            {
                Status = LoadAssemblyResult.DirectoryNotFound;
            }
            catch (IOException)
            {
                Status = LoadAssemblyResult.FailedToGetFileHandle;
            }
            catch (UnauthorizedAccessException)
            {
                Status = LoadAssemblyResult.UnauthorizedAccess;
            }

            return base.ParseBytes(rawBytes);
        }
    }
}
