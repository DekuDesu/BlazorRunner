﻿using BlazorRunner.Runner;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class InvokableMemberGroup : ComponentBase
    {
        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public IInvokableMember[] Members { get; set; }
    }
}
