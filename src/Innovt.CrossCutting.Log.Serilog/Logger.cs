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

/// <summary>
/// Implementation of <see cref="ILogger"/> and <see cref="Microsoft.Extensions.Logging.ILogger"/> using Serilog.
/// </summary>
public class Logger : ILogger, Microsoft.Extensions.Logging.ILogger
{
    /// <summary>
    /// The default output template for log messages.
    /// </summary>
    public const string DefaultOutputTemplate = "{ {timestamp:@t, ..rest(), message:@m, eventid:@i, Exception:@x} }\n";

    private global::Serilog.Core.Logger logger;


    /// <summary>
    /// Initializes a new instance of the <see cref="Logger"/> class using the default output template and Console sink.
    /// </summary>
    public Logger() : this(DefaultOutputTemplate)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Logger"/> class with a specified console output template.
    /// </summary>
    /// <param name="consoleOutputTemplate">The console output template for log messages.</param>
    public Logger(string consoleOutputTemplate = DefaultOutputTemplate)
    {
        InitializeDefaultLogger(new LoggerConfiguration(), consoleOutputTemplate: consoleOutputTemplate);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Logger"/> class with a specified log event enricher and console output template.
    /// </summary>
    /// <param name="logEventEnricher">The log event enricher to be applied.</param>
    /// <param name="consoleOutputTemplate">The console output template for log messages.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logEventEnricher"/> is null.</exception>
    public Logger(ILogEventEnricher logEventEnricher, string consoleOutputTemplate = DefaultOutputTemplate) : this(
        new[] { logEventEnricher }, consoleOutputTemplate)
    {
        if (logEventEnricher is null) throw new ArgumentNullException(nameof(logEventEnricher));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Logger"/> class with specified log event enrichers and console output template.
    /// </summary>
    /// <param name="logEventEnricher">The log event enrichers to be applied.</param>
    /// <param name="consoleOutputTemplate">The console output template for log messages.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logEventEnricher"/> is null.</exception>
    public Logger(ILogEventEnricher[] logEventEnricher, string consoleOutputTemplate = DefaultOutputTemplate)
    {
        if (logEventEnricher is null) throw new ArgumentNullException(nameof(logEventEnricher));

        InitializeDefaultLogger(new LoggerConfiguration(), logEventEnricher, consoleOutputTemplate);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Logger"/> class with a specified configuration and console output template.
    /// </summary>
    /// <param name="configuration">The Serilog logger configuration.</param>
    /// <param name="consoleOutputTemplate">The console output template for log messages.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configuration"/> is null.</exception>
    public Logger(LoggerConfiguration configuration, string consoleOutputTemplate = DefaultOutputTemplate)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        InitializeDefaultLogger(configuration, null, consoleOutputTemplate);
    }

    /// <summary>
    /// Writes a debug log message.
    /// </summary>
    /// <param name="message">The log message.</param>
    public void Debug(string message)
    {
        if (!IsEnabledInternal(LogLevel.Debug))
            return;

        logger.Debug(message);
    }

    /// <summary>
    /// Writes a debug log message using a message template and additional property values.
    /// </summary>
    /// <param name="messageTemplate">The message template for the log message.</param>
    /// <param name="propertyValues">Additional property values to include in the log message.</param>
    public void Debug(string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Debug))
            return;


        logger.Debug(messageTemplate, propertyValues);
    }

    /// <summary>
    /// Writes a debug log message with an exception and a message template.
    /// </summary>
    /// <param name="exception">The exception to include in the log message.</param>
    /// <param name="messageTemplate">The message template for the log message.</param>
    public void Debug(Exception exception, string messageTemplate)
    {
        if (!IsEnabledInternal(LogLevel.Debug))
            return;


        logger.Debug(exception, messageTemplate);
    }

    /// <summary>
    /// Writes a debug log message with an exception, a message template, and additional property values.
    /// </summary>
    /// <param name="exception">The exception to include in the log message.</param>
    /// <param name="messageTemplate">The message template for the log message.</param>
    /// <param name="propertyValues">Additional property values to include in the log message.</param>
    public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Debug))
            return;

        logger.Debug(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// Writes an error log message.
    /// </summary>
    /// <param name="message">The log message.</param>
    public void Error(string message)
    {
        if (!IsEnabledInternal(LogLevel.Error))
            return;

        logger.Error(message);
    }

    /// <summary>
    /// Writes an error log message using a message template and additional property values.
    /// </summary>
    /// <param name="messageTemplate">The message template for the log message.</param>
    /// <param name="propertyValues">Additional property values to include in the log message.</param>
    public void Error(string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Error))
            return;

        logger.Error(messageTemplate, propertyValues);
    }

    /// <summary>
    /// Writes an error log message with an exception and a message template.
    /// </summary>
    /// <param name="exception">The exception to include in the log message.</param>
    /// <param name="messageTemplate">The message template for the log message.</param>
    public void Error(Exception exception, string messageTemplate)
    {
        if (!IsEnabledInternal(LogLevel.Error))
            return;


        logger.Error(exception, messageTemplate);
    }

    /// <summary>
    /// Writes an error log message with an exception, a message template, and additional property values.
    /// </summary>
    /// <param name="exception">The exception to include in the log message.</param>
    /// <param name="messageTemplate">The message template for the log message.</param>
    /// <param name="propertyValues">Additional property values to include in the log message.</param>
    public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Error))
            return;

        logger.Error(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// Writes a fatal log message.
    /// </summary>
    /// <param name="message">The log message.</param>
    public void Fatal(string message)
    {
        if (!IsEnabledInternal(LogLevel.Critical))
            return;


        logger.Fatal(message);
    }

    /// <summary>
    /// Writes a fatal log message using a message template and additional property values.
    /// </summary>
    /// <param name="messageTemplate">The message template for the log message.</param>
    /// <param name="propertyValues">Additional property values to include in the log message.</param>
    public void Fatal(string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Critical))
            return;

        logger.Fatal(messageTemplate, propertyValues);
    }

    /// <summary>
    /// Writes a fatal log message with an exception and a message template.
    /// </summary>
    /// <param name="exception">The exception to include in the log message.</param>
    /// <param name="messageTemplate">The message template for the log message.</param>
    public void Fatal(Exception exception, string messageTemplate)
    {
        if (!IsEnabledInternal(LogLevel.Critical))
            return;

        logger.Fatal(exception, messageTemplate);
    }

    /// <summary>
    /// Writes a fatal log message with an exception, a message template, and additional property values.
    /// </summary>
    /// <param name="exception">The exception to include in the log message.</param>
    /// <param name="messageTemplate">The message template for the log message.</param>
    /// <param name="propertyValues">Additional property values to include in the log message.</param>
    public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Critical))
            return;

        logger.Fatal(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// Writes an informational log message.
    /// </summary>
    /// <param name="message">The log message.</param>
    public void Info(string message)
    {
        if (!IsEnabledInternal(LogLevel.Information))
            return;

        logger.Information(message);
    }

    /// <summary>
    /// Writes an informational log message using a message template and additional property values.
    /// </summary>
    /// <param name="messageTemplate">The message template for the log message.</param>
    /// <param name="propertyValues">Additional property values to include in the log message.</param>
    public void Info(string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Information))
            return;

        logger.Information(messageTemplate, propertyValues);
    }

    /// <summary>
    /// Writes an informational log message with an exception and a message template.
    /// </summary>
    /// <param name="exception">The exception to include in the log message.</param>
    /// <param name="messageTemplate">The message template for the log message.</param>
    public void Info(Exception exception, string messageTemplate)
    {
        if (!IsEnabledInternal(LogLevel.Information))
            return;

        logger.Information(exception, messageTemplate);
    }

    /// <summary>
    /// Writes an informational log message with an exception, a message template, and additional property values.
    /// </summary>
    /// <param name="exception">The exception to include in the log message.</param>
    /// <param name="messageTemplate">The message template for the log message.</param>
    /// <param name="propertyValues">Additional property values to include in the log message.</param>
    public void Info(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Information))
            return;

        logger.Information(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// Writes a verbose log message.
    /// </summary>
    /// <param name="message">The log message.</param>
    public void Verbose(string message)
    {
        if (!IsEnabledInternal(LogLevel.Trace))
            return;

        logger.Verbose(message);
    }

    /// <summary>
    /// Writes a verbose log message using a message template and additional property values.
    /// </summary>
    /// <param name="messageTemplate">The message template for the log message.</param>
    /// <param name="propertyValues">Additional property values to include in the log message.</param>
    public void Verbose(string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Trace))
            return;

        logger.Verbose(messageTemplate, propertyValues);
    }

    /// <summary>
    /// Writes a verbose log message with an exception and a message template.
    /// </summary>
    /// <param name="exception">The exception to include in the log message.</param>
    /// <param name="messageTemplate">The message template for the log message.</param>
    public void Verbose(Exception exception, string messageTemplate)
    {
        if (!IsEnabledInternal(LogLevel.Trace))
            return;

        logger.Verbose(exception, messageTemplate);
    }

    /// <summary>
    /// Writes a verbose log message with an exception, a message template, and additional property values.
    /// </summary>
    /// <param name="exception">The exception to include in the log message.</param>
    /// <param name="messageTemplate">The message template for the log message.</param>
    /// <param name="propertyValues">Additional property values to include in the log message.</param>
    public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Trace))
            return;

        logger.Verbose(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// Writes a warning log message.
    /// </summary>
    /// <param name="message">The log message.</param>
    public void Warning(string message)
    {
        if (!IsEnabledInternal(LogLevel.Warning))
            return;

        logger.Warning(message);
    }

    /// <summary>
    /// Writes a warning log message using a message template and additional property values.
    /// </summary>
    /// <param name="messageTemplate">The message template for the log message.</param>
    /// <param name="propertyValues">Additional property values to include in the log message.</param>
    public void Warning(string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Warning))
            return;

        logger.Warning(messageTemplate, propertyValues);
    }

    /// <summary>
    /// Writes a warning log message with an exception and a message template.
    /// </summary>
    /// <param name="exception">The exception to include in the log message.</param>
    /// <param name="messageTemplate">The message template for the log message.</param>
    public void Warning(Exception exception, string messageTemplate)
    {
        if (!IsEnabledInternal(LogLevel.Warning))
            return;

        logger.Warning(exception, messageTemplate);
    }

    /// <summary>
    /// Writes a warning log message with an exception, a message template, and additional property values.
    /// </summary>
    /// <param name="exception">The exception to include in the log message.</param>
    /// <param name="messageTemplate">The message template for the log message.</param>
    /// <param name="propertyValues">Additional property values to include in the log message.</param>
    public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        if (!IsEnabledInternal(LogLevel.Warning))
            return;

        logger.Warning(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// Writes a log message based on the specified log level, event ID, state, exception, and formatter.
    /// </summary>
    /// <typeparam name="TState">The type of the state object.</typeparam>
    /// <param name="logLevel">The log level.</param>
    /// <param name="eventId">The event ID.</param>
    /// <param name="state">The state object.</param>
    /// <param name="exception">The exception associated with the log message.</param>
    /// <param name="formatter">A delegate that formats the log message.</param>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (formatter == null) throw new ArgumentNullException(nameof(formatter));

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

    /// <summary>
    /// Checks if logging is enabled for the specified log level.
    /// </summary>
    /// <param name="logLevel">The log level to check.</param>
    /// <returns>True if logging is enabled for the specified log level; otherwise, false.</returns>
    public bool IsEnabled(LogLevel logLevel)
    {
        return IsEnabledInternal(logLevel);
    }

    /// <summary>
    /// Begins a logical operation scope.
    /// </summary>
    /// <typeparam name="TState">The type of the state object.</typeparam>
    /// <param name="state">The state object for the scope.</param>
    /// <returns>An IDisposable that ends the logical operation scope when disposed.</returns>
    public IDisposable BeginScope<TState>(TState state)
    {
        return NullScope.Instance;
    }

    /// <summary>
    /// Initializes the default logger with the specified configuration, enrichers, and console output template.
    /// </summary>
    /// <param name="configuration">The logger configuration.</param>
    /// <param name="logEventEnricher">Additional enrichers for log events.</param>
    /// <param name="consoleOutputTemplate">The template for console output.</param>
    private void InitializeDefaultLogger(LoggerConfiguration configuration, ILogEventEnricher[] logEventEnricher = null,
        string consoleOutputTemplate = DefaultOutputTemplate)
    {
        if (logger != null)
            return;

        configuration ??= new LoggerConfiguration();

        configuration.WriteTo.Console(new ExpressionTemplate(consoleOutputTemplate));

        if (logEventEnricher != null) configuration.Enrich.With(logEventEnricher);

        //default enrich
        configuration.Enrich.With(new LogLevelEnricher()).Enrich.WithActivityEnrich().Enrich.FromLogContext();

        logger = configuration.CreateLogger();
    }

    /// <summary>
    /// Determines if logging is enabled for the specified log level internally.
    /// </summary>
    /// <param name="logLevel">The log level to check.</param>
    /// <returns>True if logging is enabled for the specified log level; otherwise, false.</returns>
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