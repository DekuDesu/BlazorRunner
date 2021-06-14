using BlazorRunner.Runner.RuntimeHandling;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class ImportSettingsCard : ComponentBase
    {
        public void SaveAssemblyDirectory(object newDirectory)
        {
            if (newDirectory != null)
            {
                if (newDirectory.ToString() != AssemblyDirector.AssembliesSaveDirectory)
                {
                    SettingsDirector.Set(nameof(AssemblyDirector.AssembliesSaveDirectory), newDirectory.ToString());
                    AssemblyDirector.ReloadSettings();
                }
            }
        }
        public void SaveAssemblySourceDirectory(object newDirectory)
        {
            if (newDirectory != null)
            {
                if (newDirectory.ToString() != AssemblyDirector.ImportedSourceAssembliesDirectory)
                {
                    SettingsDirector.Set(nameof(AssemblyDirector.ImportedSourceAssembliesDirectory), newDirectory.ToString());
                    AssemblyDirector.ReloadSettings();
                }
            }
        }
    }
}
