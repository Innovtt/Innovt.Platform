using Serilog.Core;
using Serilog.Events;
using System;
using System.Diagnostics;

namespace Innovt.CrossCutting.Log.Serilog
{
    public class ActivityEnrich : ILogEventEnricher
    {

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent is null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            var activity = GetActivity();

            if (activity is null)
                return;

            logEvent.AddOrUpdateProperty(new LogEventProperty("TraceId", new ScalarValue(activity.Id)));

            if (activity.ParentId is { })
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("ParentId", new ScalarValue(activity.ParentId)));
            }

        }

#pragma warning disable CA1822 // Mark members as static
        private Activity GetActivity()
#pragma warning restore CA1822 // Mark members as static
        {
            if (Activity.Current is null)
                return null;

            return Activity.Current;

        }
    }
}
