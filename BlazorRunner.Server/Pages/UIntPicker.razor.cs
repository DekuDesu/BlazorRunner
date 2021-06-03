using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class UIntPicker : ComponentBase
    {
        [Parameter]
        public Action<uint> OnPick { get; set; }

        public uint SelectedInt
        {
            get => _SelectedInt;
            set
            {
                _SelectedInt = value;

                _InputText = value.ToString();

                OnPick?.Invoke(value);
            }
        }
        private uint _SelectedInt = 0;

        public string InputText
        {
            get => _InputText;
            set
            {
                if (uint.TryParse(value, out var parsed))
                {
                    SelectedInt = parsed;
                }
            }
        }
        private string _InputText = "";
    }
}
