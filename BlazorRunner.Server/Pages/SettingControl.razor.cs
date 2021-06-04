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

        private object Min => ((ISlider)Setting).Min;

        private object Max => ((ISlider)Setting).Max;

        private object StepSize => ((ISlider)Setting).StepSize;

        public TypeCode ParamType { get; set; }

        public void UpdateValue(object value)
        {
            Setting.Value = value;
        }

        public object GetValue() => Setting.Value;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        protected override void OnInitialized()
        {

            base.OnInitialized();

            if (Setting != null)
            {
                if (Setting.IsField)
                {
                    ParamType = Type.GetTypeCode(Setting.BackingField.FieldType);
                }
                else
                {
                    ParamType = Type.GetTypeCode(Setting.BackingProperty.PropertyType);
                }
            }
        }
    }
}
