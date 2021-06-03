using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class DoublePicker : ComponentBase
    {
        [Parameter]
        public Action<double> OnChange { get; set; }

        [Parameter]
        public double Min { get; set; } = 0d;

        [Parameter]
        public double Max { get; set; } = 1d;

        [Parameter]
        public double StepAmount { get; set; } = 0.0000000000001d;

        [Parameter]
        public double Value
        {
            get => _Value;
            set
            {
                value = Math.Clamp(value, Min, Max);

                _Value = value;

                OnChange?.Invoke(value);

                _InputText = value.ToString();
            }
        }

        public string InputText
        {
            get => _InputText;
            set
            {
                if (double.TryParse(value, out var val))
                {
                    Value = val;
                }
            }
        }

        private string _InputText = "";

        private double _Value { get; set; } = 1.0d;
    }
}
