// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;

namespace Innovt.Core.CrossCutting.Log;

/// <summary>
///     Provides an interface for logging messages with different log levels and message formatting options.
/// </summary>
public interface ILogger
{
    /// <summary>
    ///     Debug is the noisiest level, rarely (if ever) enabled for a production app.
    /// </summary>
    /// <param name="message">The message will follow the Serilog Pattern.</param>
    void Debug(string message);

    /// <summary>
    ///     Debug is the noisiest level, rarely (if ever) enabled for a production app.
    /// </summary>
    /// <param name="messageTemplate">The message templete will follow the Serilog Pattern.</param>
    /// <param name="propertyValues">Properties that will be used as template of the message</param>
    void Debug(string messageTemplate, params object[] propertyValues);

    /// <summary>
    ///     Debug is the noisiest level, rarely (if ever) enabled for a production app.
    /// </summary>
    /// <param name="exception">An Exception parameter</param>
    /// <param name="messageTemplate">The message templete will follow the Serilog Pattern.</param>
    void Debug(Exception exception, string messageTemplate);

    /// <summary>
    ///     Debug is the noisiest level, rarely (if ever) enabled for a production app.
    /// </summary>
    /// <param name="exception">An exception</param>
    /// <param name="messageTemplate">The message templete will follow the Serilog Pattern.</param>
    /// <param name="propertyValues"></param>
    void Debug(Exception exception, string messageTemplate, params object[] propertyValues);


    /// <summary>
    ///     An error logger
    /// </summary>
    /// <param name="message">The message templete will follow the Serilog Pattern.</param>
    void Error(string message);

    /// <summary>
    ///     Logs an error message with a message template and optional property values.
    /// </summary>
    /// <param name="messageTemplate">The error message template following the Serilog pattern.</param>
    /// <param name="propertyValues">Optional property values used in the message template.</param>
    void Error(string messageTemplate, params object[] propertyValues);

    /// <summary>
    ///     Logs an error message with an exception and a message template.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="messageTemplate">The error message template following the Serilog pattern.</param>
    void Error(Exception exception, string messageTemplate);

    /// <summary>
    ///     Logs an error message with an exception, a message template, and optional property values.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="messageTemplate">The error message template following the Serilog pattern.</param>
    /// <param name="propertyValues">Optional property values used in the message template.</param>
    void Error(Exception exception, string messageTemplate, params object[] propertyValues);

    /// <summary>
    ///     Logs a fatal error message.
    /// </summary>
    /// <param name="message">The fatal error message to log.</param>
    void Fatal(string message);

    /// <summary>
    ///     Logs a fatal error message with a message template and optional property values.
    /// </summary>
    /// <param name="messageTemplate">The fatal error message template following the Serilog pattern.</param>
    /// <param name="propertyValues">Optional property values used in the message template.</param>
    void Fatal(string messageTemplate, params object[] propertyValues);

    /// <summary>
    ///     Logs a fatal error message with an exception and a message template.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="messageTemplate">The fatal error message template following the Serilog pattern.</param>
    void Fatal(Exception exception, string messageTemplate);

    /// <summary>
    ///     Logs a fatal error message with an exception, a message template, and optional property values.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="messageTemplate">The fatal error message template following the Serilog pattern.</param>
    /// <param name="propertyValues">Optional property values used in the message template.</param>
    void Fatal(Exception exception, string messageTemplate, params object[] propertyValues);

    /// <summary>
    ///     Logs an information message.
    /// </summary>
    /// <param name="message">The information message to log.</param>
    void Info(string message);

    /// <summary>
    ///     Logs an information message with a message template and optional property values.
    /// </summary>
    /// <param name="messageTemplate">The information message template following the Serilog pattern.</param>
    /// <param name="propertyValues">Optional property values used in the message template.</param>
    void Info(string messageTemplate, params object[] propertyValues);

    /// <summary>
    ///     Logs an information message with an exception and a message template.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="messageTemplate">The information message template following the Serilog pattern.</param>
    void Info(Exception exception, string messageTemplate);

    /// <summary>
    ///     Logs an information message with an exception, a message template, and optional property values.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="messageTemplate">The information message template following the Serilog pattern.</param>
    /// <param name="propertyValues">Optional property values used in the message template.</param>
    void Info(Exception exception, string messageTemplate, params object[] propertyValues);

    /// <summary>
    ///     Logs a verbose message.
    /// </summary>
    /// <param name="message">The verbose message to log.</param>
    void Verbose(string message);

    /// <summary>
    ///     Logs a verbose message with a message template and optional property values.
    /// </summary>
    /// <param name="messageTemplate">The verbose message template following the Serilog pattern.</param>
    /// <param name="propertyValues">Optional property values used in the message template.</param>
    void Verbose(string messageTemplate, params object[] propertyValues);

    /// <summary>
    ///     Logs a verbose message with an exception and a message template.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="messageTemplate">The verbose message template following the Serilog pattern.</param>
    void Verbose(Exception exception, string messageTemplate);

    /// <summary>
    ///     Logs a verbose message with an exception, a message template, and optional property values.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="messageTemplate">The verbose message template following the Serilog pattern.</param>
    /// <param name="propertyValues">Optional property values used in the message template.</param>
    void Verbose(Exception exception, string messageTemplate, params object[] propertyValues);

    /// <summary>
    ///     Logs a warning message.
    /// </summary>
    /// <param name="message">The warning message to log.</param>
    void Warning(string message);

    /// <summary>
    ///     Logs a warning message with a message template and optional property values.
    /// </summary>
    /// <param name="messageTemplate">The warning message template following the Serilog pattern.</param>
    /// <param name="propertyValues">Optional property values used in the message template.</param>
    void Warning(string messageTemplate, params object[] propertyValues);

    /// <summary>
    ///     Logs a warning message with an exception and a message template.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="messageTemplate">The warning message template following the Serilog pattern.</param>
    void Warning(Exception exception, string messageTemplate);

    /// <summary>
    ///     Logs a warning message with an exception, a message template, and optional property values.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="messageTemplate">The warning message template following the Serilog pattern.</param>
    /// <param name="propertyValues">Optional property values used in the message template.</param>
    void Warning(Exception exception, string messageTemplate, params object[] propertyValues);
}

/// <summary>
///     Provides a typed logging interface that inherits from <see cref="ILogger" />.
/// </summary>
/// <typeparam name="T">The type associated with the logger.</typeparam>
public interface ILogger<T> : ILogger
{
}