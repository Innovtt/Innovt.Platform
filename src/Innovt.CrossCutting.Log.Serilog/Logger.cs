// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.Log.Serilog

using System;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Templates;
using ILogger = Innovt.Core.CrossCutting.Log.ILogger;

namespace Innovt.CrossCutting.Log.Serilog;

public class Logger : ILogger, Microsoft.Extensions.Logging.ILogger
{
    public const string DefaultOutputTemplate = "{ {timestamp:@t, ..rest(), message:@m, eventid:@i, Exception:@x} }\n";

    private global::Serilog.Core.Logger logger;

    /// <summary>
    ///     The default sink is Console
    /// </summary>
    ///
    public Logger() : this(DefaultOutputTemplate)
    {
    }

    public Logger(string consoleOutputTemplate = DefaultOutputTemplate)
    {
        InitializeDefaultLogger(new LoggerConfiguration(), consoleOutputTemplate: consoleOutputTemplate);
    }

    public Logger(ILogEventEnricher logEventEnricher, string consoleOutputTemplate = DefaultOutputTemplate) : this(
        new[] { logEventEnricher }, consoleOutputTemplate)
    {
        if (logEventEnricher is null) throw new ArgumentNullException(nameof(logEventEnricher));
    }

    public Logger(ILogEventEnricher[] logEventEnricher, string consoleOutputTemplate = DefaultOutputTemplate)
    {
        if (logEventEnricher is null) throw new ArgumentNullException(nameof(logEventEnricher));

        InitializeDefaultLogger(new LoggerConfiguration(), logEventEnricher, consoleOutputTemplate);
    }

    public Logger(LoggerConfiguration configuration, string consoleOutputTemplate = DefaultOutputTemplate)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        InitializeDefaultLogger(configuration, null, consoleOutputTemplate);
    }

    public void Debug(string message)
    {
        if (!IsEnabledInternal(LogLevel.Debug))
            return;

        logger.Debug(message);
    }

    public void Debug(string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Debug))
            return;


        logger.Debug(messageTemplate, propertyValues);
    }

    public void Debug(Exception exception, string messageTemplate)
    {
        if (!IsEnabledInternal(LogLevel.Debug))
            return;


        logger.Debug(exception, messageTemplate);
    }

    public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Debug))
            return;

        logger.Debug(exception, messageTemplate, propertyValues);
    }

    public void Error(string message)
    {
        if (!IsEnabledInternal(LogLevel.Error))
            return;

        logger.Error(message);
    }

    public void Error(string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Error))
            return;

        logger.Error(messageTemplate, propertyValues);
    }

    public void Error(Exception exception, string messageTemplate)
    {
        if (!IsEnabledInternal(LogLevel.Error))
            return;


        logger.Error(exception, messageTemplate);
    }

    public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Error))
            return;

        logger.Error(exception, messageTemplate, propertyValues);
    }

    public void Fatal(string message)
    {
        if (!IsEnabledInternal(LogLevel.Critical))
            return;


        logger.Fatal(message);
    }

    public void Fatal(string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Critical))
            return;

        logger.Fatal(messageTemplate, propertyValues);
    }

    public void Fatal(Exception exception, string messageTemplate)
    {
        if (!IsEnabledInternal(LogLevel.Critical))
            return;

        logger.Fatal(exception, messageTemplate);
    }

    public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Critical))
            return;

        logger.Fatal(exception, messageTemplate, propertyValues);
    }

    public void Info(string message)
    {
        if (!IsEnabledInternal(LogLevel.Information))
            return;

        logger.Information(message);
    }

    public void Info(string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Information))
            return;

        logger.Information(messageTemplate, propertyValues);
    }

    public void Info(Exception exception, string messageTemplate)
    {
        if (!IsEnabledInternal(LogLevel.Information))
            return;

        logger.Information(exception, messageTemplate);
    }

    public void Info(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Information))
            return;

        logger.Information(exception, messageTemplate, propertyValues);
    }

    public void Verbose(string message)
    {
        if (!IsEnabledInternal(LogLevel.Trace))
            return;

        logger.Verbose(message);
    }

    public void Verbose(string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Trace))
            return;

        logger.Verbose(messageTemplate, propertyValues);
    }

    public void Verbose(Exception exception, string messageTemplate)
    {
        if (!IsEnabledInternal(LogLevel.Trace))
            return;

        logger.Verbose(exception, messageTemplate);
    }

    public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Trace))
            return;

        logger.Verbose(exception, messageTemplate, propertyValues);
    }

    public void Warning(string message)
    {
        if (!IsEnabledInternal(LogLevel.Warning))
            return;

        logger.Warning(message);
    }

    public void Warning(string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Warning))
            return;

        logger.Warning(messageTemplate, propertyValues);
    }

    public void Warning(Exception exception, string messageTemplate)
    {
        if (!IsEnabledInternal(LogLevel.Warning))
            return;

        logger.Warning(exception, messageTemplate);
    }

    public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Warning))
            return;

        logger.Warning(exception, messageTemplate, propertyValues);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        var message = formatter(state, exception);

        switch (logLevel)
        {
            case LogLevel.Trace:
            case LogLevel.Debug:
                Debug(exception, message);
                break;
            case LogLevel.Information:
                Info(exception, message);
                break;
            case LogLevel.Warning:
                Warning(exception, message);
                break;
            case LogLevel.Error:
                Error(exception, message);
                break;
            case LogLevel.Critical:
                Fatal(exception, message);
                break;
            case LogLevel.None:
            default:
                break;
        }
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return IsEnabledInternal(logLevel);
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return NullScope.Instance;
    }

    private void InitializeDefaultLogger(LoggerConfiguration configuration, ILogEventEnricher[] logEventEnricher = null,
        string consoleOutputTemplate = DefaultOutputTemplate)
    {
        if (logger != null)
            return;

        configuration ??= new LoggerConfiguration();

        configuration.WriteTo.Console(new ExpressionTemplate(consoleOutputTemplate));

        if (logEventEnricher != null)
        {
            configuration.Enrich.With(logEventEnricher);
        }

        //default enrich
        configuration.Enrich.With(new LogLevelEnricher()).Enrich.WithActivityEnrich().Enrich.FromLogContext();

        logger = configuration.CreateLogger();
    }


    private bool IsEnabledInternal(LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Trace:
            case LogLevel.Debug:
            {
                return logger.IsEnabled(LogEventLevel.Debug) || logger.IsEnabled(LogEventLevel.Verbose);
            }
            case LogLevel.Information:
            {
                return logger.IsEnabled(LogEventLevel.Information);
            }
            case LogLevel.Warning:
            {
                return logger.IsEnabled(LogEventLevel.Warning);
            }
            case LogLevel.Error:
            {
                return logger.IsEnabled(LogEventLevel.Error);
            }

            case LogLevel.Critical:
            {
                return logger.IsEnabled(LogEventLevel.Fatal);
            }
            case LogLevel.None:
            default:
            {
                return false;
            }
        }
    }
}