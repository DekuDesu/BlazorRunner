using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class SBytePicker : ComponentBase
    {
        [Parameter]
        public Action<sbyte> OnPick { get; set; }

        public sbyte SelectedByte
        {
            get => _SelectedByte;
            set
            {
                _SelectedByte = value;
                _InputText = value.ToString();
                OnPick?.Invoke(value);
            }
        }
        private sbyte _SelectedByte = 0;

        public string InputText
        {
            get => _InputText;
            set
            {
                if (sbyte.TryParse(value, out var parsed))
                {
                    SelectedByte = parsed;
                }
            }
        }
        private string _InputText = "";
    }
}
