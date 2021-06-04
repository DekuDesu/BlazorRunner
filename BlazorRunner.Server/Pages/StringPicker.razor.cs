using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class StringPicker : ComponentBase
    {
        [Parameter]
        public Action<object> OnChange { get; set; }

        public string InputText
        {
            get => _InputText;
            set
            {
                _InputText = value;
                OnChange?.Invoke(value);
            }
        }

        private string _InputText = "";

        [Parameter]
        public object DefaultValue { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (DefaultValue != null)
            {
                InputText = DefaultValue.ToString();
            }
        }
    }
}
