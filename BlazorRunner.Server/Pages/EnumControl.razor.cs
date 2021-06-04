using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class EnumControl : ComponentBase
    {
        [Parameter]
        public Action<object> OnChange { get; set; }

        [Parameter]
        public object DefaultValue { get; set; }

        [Parameter]
        public Type EnumType { get; set; }

        private string[] Names = Array.Empty<string>();

        private object[] Values = Array.Empty<object>();

        private int[] IntValues = Array.Empty<int>();

        public object Value { get; set; } = default;

        public string SelectedName = "";

        public int Selected
        {
            get => _Selected;
            set
            {
                Value = Values[value];
                SelectedName = Names[value];
                _Selected = value;
                OnChange?.Invoke(Value);
            }
        }

        private int _Selected = 0;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (EnumType != null)
            {
                Names = Enum.GetNames(EnumType);

                var enumArr = Enum.GetValues(EnumType);

                Values = new object[enumArr.Length];

                enumArr.CopyTo(Values, 0);

                IntValues = (int[])Enum.GetValues(EnumType);
            }

            if (DefaultValue != null)
            {
                Value = DefaultValue;
            }
        }
    }
}
