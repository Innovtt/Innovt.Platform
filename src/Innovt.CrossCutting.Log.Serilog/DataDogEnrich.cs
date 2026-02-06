// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.Log.Serilog

using System;
using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Innovt.CrossCutting.Log.Serilog;

/// <summary>
///     Implementation of <see cref="ILogEventEnricher" /> that enriches log events with DataDog tracing information.
/// </summary>
public class DataDogEnrich : ILogEventEnricher
{
    /// <summary>
    ///     Enriches the provided <see cref="LogEvent" /> with DataDog tracing information.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">The property factory to create log event properties.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logEvent" /> is null.</exception>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        ArgumentNullException.ThrowIfNull(logEvent);

        var activity = GetActivity();

        if (activity is null)
            return;

        var ddTraceId = Convert.ToUInt64(activity.TraceId.ToString().Substring(16), 16).ToString();
        var ddSpanId = Convert.ToUInt64(activity.SpanId.ToString(), 16).ToString();
        logEvent.AddOrUpdateProperty(new LogEventProperty("dd.trace_id", new ScalarValue(ddTraceId)));
        logEvent.AddOrUpdateProperty(new LogEventProperty("dd.span_id", new ScalarValue(ddSpanId)));
    }

    /// <summary>
    ///     Gets the current <see cref="Activity" />.
    /// </summary>
    /// <returns>The current activity or null if no activity is available.</returns>
#pragma warning disable CA1822 // Mark members as static
    private static Activity GetActivity()
#pragma warning restore CA1822 // Mark members as static
    {
        return Activity.Current ?? null;
    }
}