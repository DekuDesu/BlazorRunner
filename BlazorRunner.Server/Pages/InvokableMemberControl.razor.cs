using BlazorRunner.Runner;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRunner.Server.Pages
{
    public partial class InvokableMemberControl : ComponentBase
    {
        [Parameter]
        public IInvokableMember Member { get; set; }
    }
}
