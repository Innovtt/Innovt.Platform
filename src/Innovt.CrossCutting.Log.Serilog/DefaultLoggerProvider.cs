using System;
using Microsoft.Extensions.Logging;

namespace Innovt.CrossCutting.Log.Serilog;

public class DefaultLoggerProvider: ILoggerProvider
{
    private readonly ILogger logger;
    private readonly Action dispose;


    public DefaultLoggerProvider(ILogger logger=null, bool dispose=false)
    {
        this.logger = logger;
    }
    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new Logger();
    }
}