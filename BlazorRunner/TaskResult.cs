using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public class TaskResult : EventArgs
    {
        public long TimeTaken { get; set; }

        public Exception Fault { get; set; }

        public bool RanToCompletion => !Faulted && !Cancelled && TimeTaken > 0;

        public bool Cancelled { get; set; }

        public bool Faulted => Fault != null;
    }
}
