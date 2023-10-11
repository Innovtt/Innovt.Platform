// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.Log.Serilog

using Serilog.Core;
using Serilog.Events;
using System;
using System.Diagnostics;

namespace Innovt.CrossCutting.Log.Serilog;

/// <summary>
/// Implementation of <see cref="ILogEventEnricher"/> that enriches log events with activity information.
/// </summary>
public class ActivityEnrich : ILogEventEnricher
{
    /// <summary>
    /// Enriches the provided <see cref="LogEvent"/> with activity information.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">The property factory to create log event properties.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logEvent"/> is null.</exception>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent is null) throw new ArgumentNullException(nameof(logEvent));

        var activity = GetActivity();

        if (activity is null)
            return;

        logEvent.AddOrUpdateProperty(new LogEventProperty("TraceId", new ScalarValue(activity.Id)));
        logEvent.AddOrUpdateProperty(new LogEventProperty("SpanId", new ScalarValue(activity.SpanId)));
        logEvent.AddOrUpdateProperty(new LogEventProperty("ParentId", new ScalarValue(activity.ParentId)));
    }

#pragma warning disable CA1822 // Mark members as static
    /// <summary>
    /// Gets the current <see cref="Activity"/>.
    /// </summary>
    /// <returns>The current activity or null if no activity is available.</returns>
    private static Activity GetActivity()
#pragma warning restore CA1822 // Mark members as static
    {
        return Activity.Current ?? null;
    }
}