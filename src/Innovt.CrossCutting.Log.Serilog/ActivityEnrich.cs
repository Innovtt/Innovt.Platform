// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.Log.Serilog

using System;
using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Innovt.CrossCutting.Log.Serilog;

public class ActivityEnrich : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent is null) throw new ArgumentNullException(nameof(logEvent));

        var activity = GetActivity();

        if (activity is null)
            return;

        logEvent.AddOrUpdateProperty(new LogEventProperty("TraceId", new ScalarValue(activity.Id)));
        logEvent.AddOrUpdateProperty(new LogEventProperty("SpanId", new ScalarValue(activity.SpanId)));
    }

#pragma warning disable CA1822 // Mark members as static
    private static Activity GetActivity()
#pragma warning restore CA1822 // Mark members as static
    {
        return Activity.Current ?? null;
    }
}