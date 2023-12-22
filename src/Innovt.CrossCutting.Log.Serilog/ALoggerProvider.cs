// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.Log.Serilog

using System;
using Microsoft.Extensions.Logging;

namespace Innovt.CrossCutting.Log.Serilog;

/// <summary>
///     Implementation of <see cref="ILoggerProvider" /> for ALogger.
/// </summary>
public class ALoggerProvider : ILoggerProvider
{
    /// <summary>
    ///     Disposes the <see cref="ALoggerProvider" />.
    /// </summary>
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Creates a new instance of <see cref="ALogger" /> for the specified category.
    /// </summary>
    /// <param name="categoryName">The category name for the logger.</param>
    /// <returns>A new instance of <see cref="ALogger" />.</returns>
    public ILogger CreateLogger(string categoryName)
    {
        throw new NotImplementedException();
    }
}