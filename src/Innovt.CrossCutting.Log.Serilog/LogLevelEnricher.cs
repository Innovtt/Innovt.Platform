// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.Log.Serilog

using System;
using Serilog.Core;
using Serilog.Events;

namespace Innovt.CrossCutting.Log.Serilog;

/// <summary>
///     Enriches Serilog log events with log4net log levels.
/// </summary>
public class LogLevelEnricher : ILogEventEnricher
{
    /// <summary>
    ///     Enriches a log event with log4net log levels.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">The log event property factory.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent is null) throw new ArgumentNullException(nameof(logEvent));

        if (propertyFactory is null)
            return;

        var log4NetLevel = logEvent.Level switch
        {
            LogEventLevel.Debug => "DEBUG",
            LogEventLevel.Error => "ERROR",
            LogEventLevel.Fatal => "FATAL",
            LogEventLevel.Information => "INFO",
            LogEventLevel.Verbose => "ALL",
            LogEventLevel.Warning => "WARN",
            _ => string.Empty
        };

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("level", log4NetLevel));
    }
}