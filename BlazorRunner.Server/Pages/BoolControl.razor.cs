using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class BoolControl : ComponentBase
    {
        [Parameter]
        public Action<object> OnChange { get; set; }

        public bool Value
        {
            get => _Value;
            set
            {
                _Value = value;
                OnChange?.Invoke(value);
            }
        }

        private bool _Value = false;

        private string ButtonColor => Value ? "btn-primary" : "btn-outline-primary";

        public void ToggleValue()
        {
            Value = !Value;
        }
    }
}
