﻿using Serilog.Core;
using Serilog.Events;
using System;

namespace Innovt.CrossCutting.Log.Serilog;

public class LogLevelEnricher : ILogEventEnricher
{
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