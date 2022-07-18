using System;

namespace Innovt.CrossCutting.Log.Serilog;

internal class NullScope : IDisposable
{
    public static NullScope Instance { get; } = new();

    public void Dispose()
    {

    }
}