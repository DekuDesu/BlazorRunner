using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Attributes
{
    public interface ISupportsGrouping
    {
        string Group { get; set; }
    }
}
