using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class BytePicker : ComponentBase
    {
        [Parameter]
        public Action<byte> OnPick { get; set; }

        public byte SelectedByte
        {
            get => _SelectedByte;
            set
            {
                _SelectedByte = value;
                _InputText = value.ToString();
                OnPick?.Invoke(value);
            }
        }
        private byte _SelectedByte = 0;

        public string InputText
        {
            get => _InputText;
            set
            {
                if (int.TryParse(value, out int parsed))
                {
                    if (parsed > 255)
                    {
                        parsed = 255;
                    }
                    if (parsed < 0)
                    {
                        parsed = 0;
                    }

                    SelectedByte = (byte)parsed;
                }
            }
        }
        private string _InputText = "";
    }
}
