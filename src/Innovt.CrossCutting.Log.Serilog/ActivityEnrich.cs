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
            
            var activityId = GetActivityId();

            if (activityId == null)
                return;

            logEvent.AddOrUpdateProperty(new LogEventProperty("TraceId",new ScalarValue(activityId)));            
        }

#pragma warning disable CA1822 // Mark members as static
        private string GetActivityId()
#pragma warning restore CA1822 // Mark members as static
        {
            if (Activity.Current is null) 
                return null;

            return Activity.Current.Id;
        }
    }
}
