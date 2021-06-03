using BlazorRunner.Runner;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class SettingControl : ComponentBase
    {
        [Parameter]
        public IScriptSetting Setting { get; set; }

        public void UpdateValue(object value)
        {
            Setting.Value = value;
        }
        public object GetValue() => Setting.Value;
    }
}
