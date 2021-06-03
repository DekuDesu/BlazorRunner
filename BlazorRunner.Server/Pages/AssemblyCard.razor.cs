using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class AssemblyCard : ComponentBase
    {
        [Parameter]
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                Acronym = BlazorRunner.Runner.Helpers.Strings.GetCodeAcronym(value) ?? value ?? "??";
                int hash = Runner.Helpers.Strings.GetPersistentHash(_Name);
                (AcronymColor, AcronymBackgroundColor) = Runner.Helpers.Colors.GetRandomColorPair(hash);
            }
        }

        [Parameter]
        public string Description { get; set; } = "";

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

        public override Task SetParametersAsync(ParameterView parameters)
        {
            Console.WriteLine("");
            return base.SetParametersAsync(parameters);
        }
    }
}
