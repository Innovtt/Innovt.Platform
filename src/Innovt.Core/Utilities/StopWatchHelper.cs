// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Diagnostics;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Core.Utilities;

public class StopWatchHelper : IDisposable
{
    private readonly string action;
    private readonly ILogger logger;
    private Stopwatch stopwatch;

    public StopWatchHelper(ILogger logger, string action)
    {
        this.logger = logger;
        this.action = action;
        stopwatch = Stopwatch.StartNew();
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

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

    ~StopWatchHelper()
    {
        Dispose(false);
    }
}