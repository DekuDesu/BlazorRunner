using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class LongPicker : ComponentBase
    {
        [Parameter]
        public Action<long> OnPick { get; set; }

        public long SelectedInt
        {
            get => _SelectedInt;
            set
            {
                _SelectedInt = value;

                _InputText = value.ToString();

                OnPick?.Invoke(value);
            }
        }
        private long _SelectedInt = 0;

        public string InputText
        {
            get => _InputText;
            set
            {
                if (long.TryParse(value, out long parsed))
                {
                    SelectedInt = parsed;
                }
            }
        }
        private string _InputText = "";
    }
}
