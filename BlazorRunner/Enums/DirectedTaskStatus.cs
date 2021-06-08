using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public enum DirectedTaskStatus
    {
        none,
        Queued,
        Running,
        Faulted,
        Cancelled,
        Finished
    }
}
