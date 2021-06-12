using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.RuntimeHandling
{
    public struct LogItem
    {
        public LogLevel logLevel;
        public EventId eventId;
        public object state;
        public Exception exception;
        public string FormattedString;

        public LogItem(LogLevel logLevel, EventId eventId, object state, Exception exception, string formattedString)
        {
            this.logLevel = logLevel;
            this.eventId = eventId;
            this.state = state;
            this.exception = exception;
            FormattedString = formattedString;
        }

        public override string ToString()
        {
            return $"{{ {nameof(logLevel)}={logLevel},{nameof(eventId)}={eventId},{nameof(state)}={state},{nameof(exception)}{exception},{nameof(FormattedString)}={FormattedString} }}";
        }
    }
}
