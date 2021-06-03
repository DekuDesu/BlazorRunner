using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class ULongPicker : ComponentBase
    {
        [Parameter]
        public Action<ulong> OnPick { get; set; }

        public ulong SelectedInt
        {
            get => _SelectedInt;
            set
            {
                _SelectedInt = value;

                _InputText = value.ToString();

                OnPick?.Invoke(value);
            }
        }
        private ulong _SelectedInt = 0;

        public string InputText
        {
            get => _InputText;
            set
            {
                if (ulong.TryParse(value, out var parsed))
                {
                    SelectedInt = parsed;
                }
            }
        }
        private string _InputText = "";
    }
}
