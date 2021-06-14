using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Attributes
{
    /// <summary>
    /// When applied to any field or property that has the type <see cref="Microsoft.Extensions.Logging.ILogger"/> within a script, it will allow outputting messages to the console. <see cref="Console"/> class will still work just fine, but outputs will onyl bne visible in the global console instead of each individual script console.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LoggerAttribute : System.Attribute
    {
    }
}
