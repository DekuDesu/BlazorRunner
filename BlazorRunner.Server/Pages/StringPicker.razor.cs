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
        public Action<string> OnChange { get; set; }

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
    }
}
