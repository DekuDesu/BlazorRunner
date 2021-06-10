using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.RuntimeHandling
{
    public static class LoggerFactory
    {
        public static ILogger CreateNewLogger()
        {
            return new UXLogger();
        }
    }
}
