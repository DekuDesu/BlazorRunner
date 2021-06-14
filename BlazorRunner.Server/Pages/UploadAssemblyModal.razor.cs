using BlazorRunner.Runner.RuntimeHandling;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
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

            if (saveToDisk)
            {
                Guid id = Guid.NewGuid();

                string path = Path.Join(AssemblyDirector.AssembliesSaveDirectory, $"{id}.dll");

                await using FileStream fs = new(path, FileMode.Create);

                await FileInfo.OpenReadStream().CopyToAsync(fs);

                if (loadImmediately)
                {
                    await AssemblyDirector.LoadAsync(path);
                }
                if (addToStartup)
                {
                    await AssemblyDirector.StartupAssemblySettings.SetAsync(id, true);
                }
            }
            else
            {
                var memoryStream = new MemoryStream();

                await FileInfo.OpenReadStream().CopyToAsync(memoryStream);

                var bytes = memoryStream.ToArray();

                await AssemblyDirector.LoadAsync(bytes);
            }

            await HideModal();
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
