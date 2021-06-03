using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class EnumControl<T> : ComponentBase where T : System.Enum
    {
        [Parameter]
        public Action<T> OnChange { get; set; }

        private readonly string[] Names = Enum.GetNames(typeof(T));

        private readonly T[] Values = (T[])Enum.GetValues(typeof(T));

        public T Value { get; set; } = default;

        public string SelectedName = "";

        public int Selected
        {
            get => _Selected;
            set
            {
                Value = Values[value];
                SelectedName = Names[value];
                _Selected = value;
            }
        }

        private int _Selected = 0;
    }
}
