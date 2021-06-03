using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class FloatPicker : ComponentBase
    {
        [Parameter]
        public Action<float> OnChange { get; set; }

        [Parameter]
        public float Min { get; set; } = 0f;

        [Parameter]
        public float Max { get; set; } = 1f;

        [Parameter]
        public float StepAmount { get; set; } = 0.000000000001f;

        [Parameter]
        public float Value
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
                if (float.TryParse(value, out var val))
                {
                    Value = val;
                }
            }
        }

        private string _InputText = "";

        private float _Value { get; set; } = 1.0f;
    }
}
