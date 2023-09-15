// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Diagnostics;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Core.Utilities;

/// <summary>
/// A helper class for measuring and logging the execution time of an action using a stopwatch.
/// </summary>
public class StopWatchHelper : IDisposable
{
    private readonly string action;
    private readonly ILogger logger;
    private Stopwatch stopwatch;

    /// <summary>
    /// Initializes a new instance of the <see cref="StopWatchHelper"/> class with a logger and an action description.
    /// </summary>
    /// <param name="logger">The logger used to log the elapsed time.</param>
    /// <param name="action">A description of the action being measured.</param>
    public StopWatchHelper(ILogger logger, string action)
    {
        this.logger = logger;
        this.action = action;
        stopwatch = Stopwatch.StartNew();
    }

    /// <summary>
    /// Releases the resources used by the <see cref="StopWatchHelper"/> instance and logs the elapsed time.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the resources used by the <see cref="StopWatchHelper"/> instance and logs the elapsed time.
    /// </summary>
    /// <param name="disposing">True if called from the <see cref="Dispose"/> method, false if called from the finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        try
        {
            logger.Info($"Action={action},ElapsedMilliseconds={stopwatch.ElapsedMilliseconds}");
            stopwatch = null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e); //todo: colocar no log
        }
    }
    /// <summary>
    /// Finalizes an instance of the <see cref="StopWatchHelper"/> class.
    /// </summary>
    ~StopWatchHelper()
    {
        Dispose(false);
    }
}