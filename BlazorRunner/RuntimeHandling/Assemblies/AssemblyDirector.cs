using BlazorRunner.Runner;
using BlazorRunner.Runner.RuntimeHandling;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
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

        public static LocalDictionary<Guid, bool> StartupAssemblySettings { get; private set; } = new("AssemblyStartup.json");

        public static LocalDictionary<Guid, string> AssemblyInfoDict { get; private set; } = new("AssemblyInfo.json");

        public static HashSet<Guid> ImportedAssemblies { get; private set; } = new();

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
                int num = -1;
                await foreach (var assembly in enumerableImporter)
                {
                    Load(assembly);

                    var pathIndex = Interlocked.Increment(ref num);

                    if (assembly != null)
                    {
                        if (TryParsePath(Paths[pathIndex], out var id))
                        {
                            if (AssemblyInfoDict.ContainsKey(id) is false)
                            {
                                await AssemblyInfoDict.SetAsync(id, assembly.FullName);
                            }
                            ImportedAssemblies.Add(id);
                        }
                    }
                }
            }
            else
            {
                Assembly assembly = await importer.LoadAsync();

                Load(assembly);

                if (TryParsePath(Paths[0], out var id))
                {
                    if (AssemblyInfoDict.ContainsKey(id) is false)
                    {
                        await AssemblyInfoDict.SetAsync(id, assembly.FullName);
                    }
                    ImportedAssemblies.Add(id);
                }
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
            await AssemblyInfoDict.LoadAsync();

            // load where we should look
            VerifyDirectory(nameof(AssembliesSaveDirectory), ref _AssembliesSaveDirectory);

            VerifyDirectory(nameof(ImportedSourceAssembliesDirectory), ref _ImportedSourceAssembliesDirectory);

            // get all the files
            var files = Directory.GetFiles(AssembliesSaveDirectory);

            List<string> pathsToLoad = new();

            // gety the name of each one and check the startup assembly dict to see if we should load it
            foreach (var path in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(path);

                if (TryParsePath(path, out var id))
                {
                    if (StartupAssemblySettings.ContainsKey(id))
                    {
                        if (StartupAssemblySettings.Get(id))
                        {
                            pathsToLoad.Add(path);
                        }
                        continue;
                    }
                }

                if (fileName != null)
                {
                    pathsToLoad.Add(path);

                    var newId = DuplicateFile(path);

                    await StartupAssemblySettings.SetAsync(newId, true);
                }
            }

            await LoadAsync(pathsToLoad.ToArray());
        }

        private static bool TryParsePath(string path, out Guid Id)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);

            if (fileName == null)
            {
                Id = default;
                return false;
            }
            return Guid.TryParse(fileName, out Id);
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

        public static void ReloadSettings()
        {
            AssembliesSaveDirectory = SettingsDirector.Get<string>(nameof(AssembliesSaveDirectory));
            ImportedSourceAssembliesDirectory = SettingsDirector.Get<string>(nameof(ImportedSourceAssembliesDirectory));
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
                try
                {
                    item?.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(LogExceptions(e));
                }
            }
        }
        private static string LogExceptions(Exception e)
        {
            string inner = e.InnerException != null ? LogExceptions(e.InnerException) : "";
            return $"<pre><div>{e.Message}</div><div>    {e.StackTrace}</div></pre>{inner}";
        }
    }
}
