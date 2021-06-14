using BlazorRunner.Runner;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class ScriptSelectCard : ComponentBase
    {
        [Parameter]
        public string Name
        {
            get => _Name;
            set
            {
                if (value == null)
                {
                    return;
                }
                _Name = value;
                Acronym = BlazorRunner.Runner.Helpers.Strings.GetCodeAcronym(value) ?? value ?? "??";
                int hash = Runner.Helpers.Strings.GetPersistentHash(_Name);
                (AcronymColor, AcronymBackgroundColor) = Runner.Helpers.Colors.GetRandomColorPair(hash);
            }
        }

        [Parameter]
        public string Description { get; set; } = "";

        [Parameter]
        public int Scripts { get; set; } = 0;

        [Parameter]
        public int Settings { get; set; } = 0;

        [Parameter]
        public IScript Script { get; set; }

        [Parameter]
        public Action OnViewClick { get; set; }

        public string Acronym = "";

        private string _Name = "";

        string AcronymColor = "rgb(0,0,0)";
        string AcronymBackgroundColor = "rgb(255,255,255)";

        string BubbleText => ShowAcronym ? Acronym : "<span class=\"oi oi-magnifying-glass\"></span>";

        bool ShowAcronym = true;

        public void ShowBubbleText() => ShowAcronym = false;

        public void HideBubbleText() => ShowAcronym = true;

        bool ShowRunAll = false;

        public void ShowRunAllButton() => ShowRunAll = true;

        public void HideRunAllButton() => ShowRunAll = false;

        public void HideAll()
        {
            ShowAcronym = true;
            ShowRunAll = false;
        }

        public async Task InvokeAll()
        {
            await TaskDirector.QueueTask(Script.ToAction(), Script.Id, Script.Logger, Script.Name);
        }

        public void SelectAssembly()
        {
            OnViewClick?.Invoke();
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            return base.SetParametersAsync(parameters);
        }
    }
}
