using BlazorRunner.Runner;
using BlazorRunner.Runner.RuntimeHandling;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.RuntimeHandling
{
    public static class AssemblyDirector
    {
        private static string _ImportedSourceAssembliesDirectory = "/Assemblies/Imported/";
        private static string _AssembliesSaveDirectory = "/Assemblies";

        public static IDictionary<Guid, IScriptAssembly> LoadedAssemblies { get; set; } = new ConcurrentCallbackDictionary<Guid, IScriptAssembly>();

        public static IAssemblyBuilder Builder { get; private set; } = Factory.CreateAssemblyBuilder();

        public static string AssembliesSaveDirectory { get => _AssembliesSaveDirectory; set => _AssembliesSaveDirectory = value; }
        public static string ImportedSourceAssembliesDirectory { get => _ImportedSourceAssembliesDirectory; set => _ImportedSourceAssembliesDirectory = value; }
        public static string[] StartingAssemblyPaths { get; set; } = Array.Empty<string>();

        public static byte[][] StartingAssemblyBytes { get; set; } = Array.Empty<byte[]>();

        public static Assembly[] StartingAssembies { get; set; } = Array.Empty<Assembly>();

        public static bool LoadedStartupAssemblies { get; private set; } = false;

        public static LocalDictionary<Guid, bool> StartupAssemblySettings { get; private set; } = new("StartupAssemblies.json");

        public static IScriptAssembly[] GetAssemblies()
        {
            var values = LoadedAssemblies.Values;

            IScriptAssembly[] vals = new IScriptAssembly[values.Count];

            values.CopyTo(vals, 0);

            return vals;
        }

        public static async Task<bool> LoadStartupAssemblies()
        {
            if (LoadedStartupAssemblies)
            {
                return true;
            }

            try
            {
                await LoadLocal();

                await LoadAsync(StartingAssemblyPaths);

                await LoadAsync(StartingAssemblyBytes);

                Load(StartingAssembies);

                LoadedStartupAssemblies = true;

                return true;
            }
            catch (Exception e)
            {
                Console.Error.Write(e);
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

        public static async Task LoadLocal()
        {
            // load the dict so we can check which assemblies to load
            await StartupAssemblySettings.LoadAsync();

            // load where we should look
            VerifyDirectory(nameof(AssembliesSaveDirectory), ref _AssembliesSaveDirectory);

            // get all the files
            var files = Directory.GetFiles(AssembliesSaveDirectory);

            List<string> pathsToLoad = new();

            // gety the name of each one and check the startup assembly dict to see if we should load it
            foreach (var path in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(path);

                if (fileName != null)
                {
                    if (Guid.TryParse(fileName, out var id))
                    {
                        if (StartupAssemblySettings.ContainsKey(id))
                        {
                            if (StartupAssemblySettings.Get(id))
                            {
                                pathsToLoad.Add(path);
                            }
                            continue;
                        }
                        continue;
                    }

                    pathsToLoad.Add(path);

                    var newId = DuplicateFile(path);

                    await StartupAssemblySettings.SetAsync(newId, true);
                }
            }

            await LoadAsync(pathsToLoad.ToArray());
        }

        private static Guid DuplicateFile(string source)
        {
            if (File.Exists(source) is false)
            {
                return default;
            }
            Guid newName = Guid.NewGuid();

            string path = Path.Join(Path.GetDirectoryName(source), $"{newName}{Path.GetExtension(source)}");

            File.Copy(source, path);

            VerifyDirectory(nameof(ImportedSourceAssembliesDirectory), ref _ImportedSourceAssembliesDirectory);

            File.Move(source, Path.Combine(ImportedSourceAssembliesDirectory, Path.GetFileName(source)));

            return newName;
        }

        private static void VerifyDirectory(string Key, ref string Value)
        {
            var found = SettingsDirector.Get<string>(Key);
            if (found == null)
            {
                var dir = Directory.GetCurrentDirectory();

                string newPath = Path.Join(dir, Value);

                Value = newPath;

                SettingsDirector.Set(Key, Value);
            }
            else
            {
                Value = found;
            }

            if (Directory.Exists(Value) is false)
            {
                Directory.CreateDirectory(Value);
            }
        }

        private static T LoadOrSave<T>(string Key, ref T Value)
        {
            var loadedValue = SettingsDirector.Get<T>(Key);

            if (loadedValue == null)
            {
                SettingsDirector.Set(Key, Value);
            }
            else
            {
                Value = loadedValue;
            }

            return Value;
        }

        public static void Load(params Assembly[] assemblies)
        {
            for (int i = 0; i < assemblies.Length; i++)
            {
                IScriptAssembly parsedAssembly = Builder.Parse(assemblies[i]);

                if (parsedAssembly != null)
                {
                    LoadedAssemblies.Add(parsedAssembly.Id, parsedAssembly);
                }
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
