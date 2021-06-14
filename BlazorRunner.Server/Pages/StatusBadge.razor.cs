using BlazorRunner.Runner;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class StatusBadge : ComponentBase
    {
        [Parameter]
        public DirectedTaskStatus DirectedStatus { get; set; }

        [Parameter]
        public TaskStatus Status { get; set; }

        [Parameter]
        public bool UseSystemStatusInstead { get; set; } = false;
    }
}
