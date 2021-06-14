using Microsoft.AspNetCore.Components;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class PrimitiveSlider<T> : ComponentBase where T : struct, IConvertible, IComparable<T>
    {
        [Parameter]
        public Action<object> OnChange { get; set; }

        [Parameter]
        public bool EnableSlider { get; set; } = true;

        [Parameter]
        public bool EnableBitShift { get; set; } = true;

        [Parameter]
        public bool EnableIncrement { get; set; } = true;

        [Parameter]
        public bool EnableSliderConstraints { get; set; } = true;

        [Parameter]
        public object Min { get; set; } = default(T);

        [Parameter]
        public object Max { get; set; } = default(T);

        [Parameter]
        public object StepAmount { get; set; } = default(T);

        [Parameter]
        public int LimitTextInputLength { get; set; } = -1;

        [Parameter]
        public Type SliderTextTypeOverride { get; set; } = null;

        [Parameter]
        public T Value
        {
            get => _Value;
            set
            {
                if (EnableSliderConstraints)
                {
                    if (value.CompareTo((dynamic)Min) < 0)
                    {
                        value = (dynamic)Min;
                    }
                    else if (value.CompareTo((dynamic)Max) > 0)
                    {
                        value = (dynamic)Max;
                    }
                }

                _Value = value;

                OnChange?.Invoke(value);

                _InputText = value.ToString();

                if (value is char c)
                {
                    _SliderValue = ((int)c).ToString();
                }
                else
                {
                    _SliderValue = value.ToString();
                }

            }
        }

        private T _Value { get; set; } = default;

        [Parameter]
        public object DefaultValue { get; set; }

        public string InputText
        {
            get => _InputText;
            set
            {
                if (LimitTextInputLength > -1)
                {
                    if (value.Length > LimitTextInputLength)
                    {
                        value = value[0..LimitTextInputLength];
                    }
                }
                if (BlazorRunner.Runner.TypeValidator.TryGetCompatibility(value, typeof(T), out var compatibility))
                {
                    Value = (T)Runner.TypeValidator.Cast(value, typeof(T), compatibility);
                }
            }
        }

        private string _InputText = "";

        private string SliderTypeName = "";

        public string SliderValue
        {
            get => _SliderValue;
            set
            {
                // this probably another war crime
                if (SliderTextTypeOverride != null)
                {
                    if (BlazorRunner.Runner.TypeValidator.TryGetCompatibility(value, SliderTextTypeOverride, out var compatibility))
                    {
                        var tmp = Runner.TypeValidator.Cast(value, SliderTextTypeOverride, compatibility);
                        if (BlazorRunner.Runner.TypeValidator.TryGetCompatibility(tmp, typeof(T), out compatibility))
                        {
                            DynamicValue = (T)Runner.TypeValidator.Cast(tmp, typeof(T), compatibility);
                        }
                    }
                }
                else if (BlazorRunner.Runner.TypeValidator.TryGetCompatibility(value, typeof(T), out var compatibility))
                {
                    DynamicValue = (T)Runner.TypeValidator.Cast(value, typeof(T), compatibility);
                }
            }
        }

        public string _SliderValue = default(T).ToString();

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

            if (DefaultValue != null)
            {
                DynamicValue = DefaultValue;
            }
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
            // this is a war crime i'm sure of it
            // this monstrocity was the only way i could make all types without knowing their types with this stupid component
            try
            {
                DynamicValue <<= 1;
            }
            catch (RuntimeBinderException e)
            {
                // forgive me
                if (e.Message.StartsWith("Cannot implicitly convert type 'int' to"))
                {
                    try
                    {
                        int shiftedVal = ((int)DynamicValue) << 1;

                        if (BlazorRunner.Runner.TypeValidator.TryGetCompatibility(shiftedVal, typeof(T), out var compatibility))
                        {
                            DynamicValue = (T)Runner.TypeValidator.Cast(shiftedVal, typeof(T), compatibility);
                        }

                    }
                    catch (Exception) { }
                }
            }
        }

        public void BitShiftRight()
        {
            // this is a war crime i'm sure of it
            try
            {
                DynamicValue >>= 1;
            }
            catch (RuntimeBinderException e)
            {
                if (e.Message.StartsWith("Cannot implicitly convert type 'int' to"))
                {
                    try
                    {
                        int shiftedVal = ((int)DynamicValue) >> 1;

                        if (BlazorRunner.Runner.TypeValidator.TryGetCompatibility(shiftedVal, typeof(T), out var compatibility))
                        {
                            DynamicValue = (T)Runner.TypeValidator.Cast(shiftedVal, typeof(T), compatibility);
                        }
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
