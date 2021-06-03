using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class PrimitiveSlider<T> : ComponentBase where T : struct, IConvertible, IComparable<T>
    {
        [Parameter]
        public Action<T> OnChange { get; set; }

        [Parameter]
        public bool EnableSlider { get; set; } = true;

        [Parameter]
        public bool EnableBitShift { get; set; } = true;

        [Parameter]
        public bool EnableIncrement { get; set; } = true;

        [Parameter]
        public T Min { get; set; } = default;

        [Parameter]
        public T Max { get; set; } = default;

        [Parameter]
        public T StepAmount { get; set; } = default;

        [Parameter]
        public T Value
        {
            get => _Value;
            set
            {
                if (value.CompareTo(Min) < 0)
                {
                    value = Min;
                }
                else if (value.CompareTo(Max) > 0)
                {
                    value = Max;
                }

                _Value = value;

                OnChange?.Invoke(value);

                _InputText = value.ToString();

                StateHasChanged();
            }
        }

        private T _Value { get; set; } = default;

        public string InputText
        {
            get => _InputText;
            set
            {
                if (BlazorRunner.Runner.TypeValidator.TryGetCompatibility(value, typeof(T), out var compatibility))
                {
                    Value = (T)Runner.TypeValidator.Cast(value, typeof(T), compatibility);
                }
            }
        }

        private string _InputText = "";

        private string SliderTypeName = "";

        public dynamic DynamicValue
        {
            get => Value;
            set
            {
                Value = value;
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            SliderTypeName = typeof(T).Name;
        }

        public void Increment()
        {
            try
            {
                DynamicValue++;
            }
            catch (Exception) { }
        }

        public void Decrement()
        {
            try
            {
                DynamicValue--;
            }
            catch (Exception) { }
        }

        public void BitShiftLeft()
        {
            try
            {
                DynamicValue <<= 1;
            }
            catch (Exception) { }
        }

        public void BitShiftRight()
        {
            try
            {
                DynamicValue >>= 1;
            }
            catch (Exception) { }
        }
    }
}
