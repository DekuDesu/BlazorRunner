using BlazorRunner.Runner;
using BlazorRunner.Runner.RuntimeHandling;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.RuntimeHandling
{
    public static class AssemblyDirector
    {
        public static IDictionary<Guid, IScriptAssembly> LoadedAssemblies { get; set; } = new ConcurrentCallbackDictionary<Guid, IScriptAssembly>();

        public static IAssemblyBuilder Builder { get; private set; } = Factory.CreateAssemblyBuilder();

        public static string[] StartingAssemblyPaths { get; set; } = Array.Empty<string>();

        public static byte[][] StartingAssemblyBytes { get; set; } = Array.Empty<byte[]>();

        public static Assembly[] StartingAssembies { get; set; } = Array.Empty<Assembly>();

        public static bool LoadedStartupAssemblies { get; private set; } = false;

        public static async Task<bool> LoadStartupAssemblies()
        {
            if (LoadedStartupAssemblies)
            {
                return true;
            }

            try
            {
                await LoadAsync(StartingAssemblyPaths);

                await LoadAsync(StartingAssemblyBytes);

                Load(StartingAssembies);

                LoadedStartupAssemblies = true;

                return true;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return false;
            }

        }

        public static void Load(params string[] Paths)
        {
            IAssemblyImporter importer = Factory.CreateImporter(Paths);

            if (importer is IEnumerable<Assembly> enumerableImporter)
            {
                foreach (var assembly in enumerableImporter)
                {
                    Load(assembly);
                }
            }
            else
            {
                Assembly assembly = importer.Load();

                Load(assembly);
            }
        }

        public static void Load(params byte[][] bytes)
        {
            IAssemblyImporter importer = Factory.CreateImporter(bytes);

            if (importer is IEnumerable<Assembly> enumerableImporter)
            {
                foreach (var assembly in enumerableImporter)
                {
                    Load(assembly);
                }
            }
            else
            {
                Assembly assembly = importer.Load();

                Load(assembly);
            }
        }

        public static async Task LoadAsync(params string[] Paths)
        {
            IAssemblyImporter importer = Factory.CreateImporter(Paths);

            if (importer is IAsyncEnumerable<Assembly> enumerableImporter)
            {
                await foreach (var assembly in enumerableImporter)
                {
                    Load(assembly);
                }
            }
            else
            {
                Assembly assembly = await importer.LoadAsync();

                Load(assembly);
            }
        }

        public static async Task LoadAsync(params byte[][] bytes)
        {
            IAssemblyImporter importer = Factory.CreateImporter(bytes);

            if (importer is IAsyncEnumerable<Assembly> enumerableImporter)
            {
                await foreach (var assembly in enumerableImporter)
                {
                    Load(assembly);
                }
            }
            else
            {
                Assembly assembly = await importer.LoadAsync();

                Load(assembly);
            }
        }

        public static void Load(params Assembly[] assemblies)
        {
            for (int i = 0; i < assemblies.Length; i++)
            {
                IScriptAssembly parsedAssembly = Builder.Parse(assemblies[i]);

                LoadedAssemblies.Add(parsedAssembly.Id, parsedAssembly);
            }
        }

        public static void Unload(Guid AssemblyId)
        {
            LoadedAssemblies.Remove(AssemblyId);
        }

        public static void UnloadAll()
        {
            LoadedStartupAssemblies = false;

            var items = LoadedAssemblies.Values;

            LoadedAssemblies.Clear();

            foreach (var item in items)
            {
                item?.Dispose();
            }
        }
    }
}
