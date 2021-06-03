using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class ShortPicker : ComponentBase
    {
        [Parameter]
        public Action<short> OnPick { get; set; }

        public short SelectedInt
        {
            get => _SelectedInt;
            set
            {
                _SelectedInt = value;

                _InputText = value.ToString();

                OnPick?.Invoke(value);
            }
        }
        private short _SelectedInt = 0;

        public string InputText
        {
            get => _InputText;
            set
            {
                if (short.TryParse(value, out var parsed))
                {
                    SelectedInt = parsed;
                }
            }
        }
        private string _InputText = "";
    }
}
