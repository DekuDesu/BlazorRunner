using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class DecimalPicker : ComponentBase
    {
        [Parameter]
        public Action<decimal> OnChange { get; set; }

        [Parameter]
        public decimal Min { get; set; } = 0m;

        [Parameter]
        public decimal Max { get; set; } = 1m;

        [Parameter]
        public decimal StepAmount { get; set; } = 0.000000000000000001m;

        [Parameter]
        public decimal Value
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
                if (decimal.TryParse(value, out var val))
                {
                    Value = val;
                }
            }
        }

        private string _InputText = "";

        private decimal _Value { get; set; } = 1.0m;
    }
}
