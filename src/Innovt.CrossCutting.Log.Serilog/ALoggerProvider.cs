using Microsoft.Extensions.Logging;

namespace Innovt.CrossCutting.Log.Serilog;

public class ALoggerProvider: ILoggerProvider
{
    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public ILogger CreateLogger(string categoryName)
    {
        throw new System.NotImplementedException();
    }
}