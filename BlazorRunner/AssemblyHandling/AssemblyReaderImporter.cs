using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    /// <summary>
    /// Imports assemblies from local files using the provided path
    /// </summary>
    public class AssemblyReaderImporter : BaseAssemblyImporter, IEnumerable<Assembly>, IAsyncEnumerable<Assembly>
    {
        public string[] Paths { get; set; }

        public int CurrentIndex { get; set; } = 0;

        public AssemblyReaderImporter(params string[] Paths)
        {
            this.Paths = Paths;
        }

        public override Assembly Load()
        {
            byte[] rawBytes = null;

            // attempt to read the file
            try
            {
                rawBytes = File.ReadAllBytes(Paths[CurrentIndex]);
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
                rawBytes = await File.ReadAllBytesAsync(Paths[CurrentIndex]);
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

        public IEnumerator<Assembly> GetEnumerator()
        {
            for (int i = 0; i < Paths.Length; i++)
            {
                yield return Load();
                CurrentIndex++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public async IAsyncEnumerator<Assembly> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            for (int i = 0; i < Paths.Length; i++)
            {
                yield return await LoadAsync();
                CurrentIndex++;
            }
        }
    }
}
