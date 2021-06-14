using BlazorRunner.Runner.RuntimeHandling;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class AssemblyStartupSettingsCard : ComponentBase
    {

        public void SetValue(Guid id, bool value)
        {
            AssemblyDirector.StartupAssemblySettings.Set(id, value);
            Console.WriteLine($"Toggled {id}, to {value}");
        }

        public void Load(Guid id)
        {
            if (AssemblyDirector.ImportedAssemblies.Contains(id) is false)
            {
                string path = Path.Join(AssemblyDirector.AssembliesSaveDirectory, $"{id}.dll");
                AssemblyDirector.Load(path);
                Console.WriteLine($"Loaded {path}");
            }
        }

        public void UnloadAll()
        {
            AssemblyDirector.UnloadAll();
        }
    }
}
