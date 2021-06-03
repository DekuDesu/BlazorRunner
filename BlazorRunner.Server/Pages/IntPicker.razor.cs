using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class IntPicker : ComponentBase
    {
        [Parameter]
        public Action<int> OnPick { get; set; }

        public int SelectedInt
        {
            get => _SelectedInt;
            set
            {
                _SelectedInt = value;

                _InputText = value.ToString();

                OnPick?.Invoke(value);
            }
        }
        private int _SelectedInt = 0;

        public string InputText
        {
            get => _InputText;
            set
            {
                if (int.TryParse(value, out int parsed))
                {
                    SelectedInt = parsed;
                }
            }
        }
        private string _InputText = "";
    }
}
