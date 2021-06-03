using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class UShortPicker : ComponentBase
    {
        [Parameter]
        public Action<ushort> OnPick { get; set; }

        public ushort SelectedInt
        {
            get => _SelectedInt;
            set
            {
                _SelectedInt = value;

                _InputText = value.ToString();

                OnPick?.Invoke(value);
            }
        }
        private ushort _SelectedInt = 0;

        public string InputText
        {
            get => _InputText;
            set
            {
                if (ushort.TryParse(value, out var parsed))
                {
                    SelectedInt = parsed;
                }
            }
        }
        private string _InputText = "";
    }
}
