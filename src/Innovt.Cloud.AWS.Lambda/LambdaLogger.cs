// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda

using System;
using System.Globalization;
using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda;

internal class LambdaLogger : ILogger
{
    private readonly IFormatProvider formatProvider;
    private readonly ILambdaLogger lambdaLogger;

    public LambdaLogger(ILambdaLogger lambdaLogger, IFormatProvider formatProvider = null)
    {
        this.lambdaLogger = lambdaLogger ?? throw new ArgumentNullException(nameof(lambdaLogger));

        this.formatProvider = formatProvider ?? CultureInfo.CurrentCulture;
    }

    public void Debug(string message)
    {
        lambdaLogger.LogLine($"DEBUG: Message: {message}");
    }

    public void Debug(string messageTemplate, params object[] propertyValues)
    {
        lambdaLogger.LogLine($"DEBUG: Message: {string.Format(formatProvider, messageTemplate, propertyValues)}");
    }


    public void Debug(Exception exception, string messageTemplate)
    {
        lambdaLogger.LogLine(
            $"DEBUG: Message: {messageTemplate}, Exception: {exception.Message}, Stacktrace: {exception.StackTrace}");
    }

    public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        lambdaLogger.LogLine(
            $"DEBUG: Message:{string.Format(formatProvider, messageTemplate, propertyValues)}, Exception: {exception.Message}, Stacktrace: {exception.StackTrace}");
    }


    public void Error(string message)
    {
        lambdaLogger.LogLine($"ERROR: Message: {message}");
    }


    public void Error(string messageTemplate, params object[] propertyValues)
    {
        lambdaLogger.LogLine($"ERROR: Message: {string.Format(formatProvider, messageTemplate, propertyValues)}");
    }


    public void Error(Exception exception, string messageTemplate)
    {
        lambdaLogger.LogLine(
            $"ERROR: Message: {messageTemplate}, Exception: {exception.Message}, Stacktrace: {exception.StackTrace}");
    }


    public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        lambdaLogger.LogLine(
            $"DEBUG: Message:{string.Format(formatProvider, messageTemplate, propertyValues)}, Exception: {exception.Message}, Stacktrace: {exception.StackTrace}");
    }


    public void Fatal(string message)
    {
        lambdaLogger.LogLine($"FATAL: Message: {message}");
    }


    public void Fatal(string messageTemplate, params object[] propertyValues)
    {
        lambdaLogger.LogLine($"FATAL: Message: {string.Format(formatProvider, messageTemplate, propertyValues)}");
    }


    public void Fatal(Exception exception, string messageTemplate)
    {
        lambdaLogger.LogLine(
            $"FATAL: Message: {messageTemplate}, Exception: {exception.Message}, Stacktrace: {exception.StackTrace}");
    }


    public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        lambdaLogger.LogLine(
            $"FATAL: Message:{string.Format(formatProvider, messageTemplate, propertyValues)}, Exception: {exception.Message}, Stacktrace: {exception.StackTrace}");
    }


    public void Info(string message)
    {
        lambdaLogger.LogLine($"INFO: Message: {message}");
    }


    public void Info(string messageTemplate, params object[] propertyValues)
    {
        lambdaLogger.LogLine($"INFO: Message: {string.Format(formatProvider, messageTemplate, propertyValues)}");
    }


    public void Info(Exception exception, string messageTemplate)
    {
        lambdaLogger.LogLine(
            $"INFO: Message: {messageTemplate}, Exception: {exception.Message}, Stacktrace: {exception.StackTrace}");
    }


    public void Info(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        lambdaLogger.LogLine(
            $"FATAL: Message:{string.Format(formatProvider, messageTemplate, propertyValues)}, Exception: {exception.Message}, Stacktrace: {exception.StackTrace}");
    }


    public void Verbose(string message)
    {
        lambdaLogger.LogLine($"VERBOSE: Message: {message}");
    }


    public void Verbose(string messageTemplate, params object[] propertyValues)
    {
        lambdaLogger.LogLine($"VERBOSE: Message: {string.Format(formatProvider, messageTemplate, propertyValues)}");
    }


    public void Verbose(Exception exception, string messageTemplate)
    {
        lambdaLogger.LogLine(
            $"VERBOSE: Message: {messageTemplate}, Exception: {exception.Message}, Stacktrace: {exception.StackTrace}");
    }


    public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        lambdaLogger.LogLine(
            $"VERBOSE: Message:{string.Format(formatProvider, messageTemplate, propertyValues)}, Exception: {exception.Message}, Stacktrace: {exception.StackTrace}");
    }


    public void Warning(string message)
    {
        lambdaLogger.LogLine($"WARNING: Message: {message}");
    }


    public void Warning(string messageTemplate, params object[] propertyValues)
    {
        lambdaLogger.LogLine($"WARNING: Message: {string.Format(formatProvider, messageTemplate, propertyValues)}");
    }

    public void Warning(Exception exception, string messageTemplate)
    {
        lambdaLogger.LogLine(
            $"WARNING: Message: {messageTemplate}, Exception: {exception.Message}, Stacktrace: {exception.StackTrace}");
    }


    public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        lambdaLogger.LogLine(
            $"WARNING: Message:{string.Format(formatProvider, messageTemplate, propertyValues)}, Exception: {exception.Message}, Stacktrace: {exception.StackTrace}");
    }
}