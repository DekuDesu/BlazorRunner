using BlazorRunner.Runner;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class SettingGroup : ComponentBase
    {
        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public IScriptSetting[] Settings { get; set; }
    }
}
