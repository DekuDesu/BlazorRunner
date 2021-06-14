using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class UploadAssemblyModal : ComponentBase
    {
        [Parameter]
        public IBrowserFile FileInfo { get; set; }

        bool saveToDisk = true;
        bool loadImmediately = true;
        bool addToStartup = true;

        private async Task Upload()
        {
            await Task.Delay(1);
        }

        private void EnableSave()
        {
            saveToDisk = true;
        }

        private void DisableSave()
        {
            saveToDisk = false;
            addToStartup = false;
            StateHasChanged();
        }

        private async Task ShowModal()
        {
            await Runtime.InvokeVoidAsync("toggle_modal", "staticBackdrop", true);
        }
        private async Task HideModal()
        {
            await Runtime.InvokeVoidAsync("toggle_modal", "staticBackdrop", false);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ShowModal();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }
    }
}
