using OpenTracing.Util;
using Serilog.Core;
using Serilog.Events;

namespace Innovt.CrossCutting.Log.Serilog
{
    //this code came from https://github.com/yesmarket/Serilog.Enrichers.OpenTracing/blob/master/src/Serilog.Enrichers.OpenTracing/OpenTracingContextLogEventEnricher.cs
    public class OpenTracingContextLogEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var tracer = GlobalTracer.Instance;

            if (tracer?.ActiveSpan == null)
                return;

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", tracer.ActiveSpan.Context.TraceId));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", tracer.ActiveSpan.Context.SpanId));
        }
    }
}