using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class GuidPicker : ComponentBase
    {
        [Parameter]
        public Action<object> OnChange { get; set; }

        public string InputText
        {
            get => _InputText;
            set
            {
                if (TryParseGuid(value, out var newId))
                {
                    Id = newId;
                    _InputText = value;
                }
            }
        }
        private string _InputText = "";

        [Parameter]
        public object DefaultValue { get; set; }

        public Guid Id
        {
            get => _id;
            set
            {
                OnChange?.Invoke(value);
                _id = value;
            }
        }
        private Guid _id;

        public void Randomize()
        {
            InputText = Guid.NewGuid().ToString();
        }

        public bool TryParseGuid(string id, out Guid newId)
        {
            try
            {
                newId = new Guid(id);
                return true;
            }
            catch (ArgumentNullException)
            {
                newId = default;
                return false;
            }
            catch (FormatException)
            {
                newId = default;
                return false;
            }
            catch (OverflowException)
            {
                newId = default;
                return false;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (DefaultValue != null)
            {
                Id = (Guid)DefaultValue;
            }
        }
    }
}
