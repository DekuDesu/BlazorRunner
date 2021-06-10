using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.AssemblyHandling
{
    public class ByteAssemblyImporter : BaseAssemblyImporter, IEnumerable<Assembly>, IAsyncEnumerable<Assembly>
    {
        private byte[][] Bytes { get; set; } = null;

        public int CurrentIndex { get; set; } = 0;

        public ByteAssemblyImporter(params byte[][] bytes)
        {
            Bytes = bytes;
        }

        public override async Task<Assembly> LoadAsync()
        {
            await Task.Delay(1);

            return base.ParseBytes(Bytes[CurrentIndex]);
        }

        public override Assembly Load()
        {
            return base.ParseBytes(Bytes[CurrentIndex]);
        }

        public IEnumerator<Assembly> GetEnumerator()
        {
            for (int i = 0; i < Bytes.Length; i++)
            {
                yield return Load();
                CurrentIndex++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public async IAsyncEnumerator<Assembly> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            for (int i = 0; i < Bytes.Length; i++)
            {
                yield return await LoadAsync();
                CurrentIndex++;
            }
        }
    }
}
