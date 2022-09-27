// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.Log.Serilog

using System;

namespace Innovt.CrossCutting.Log.Serilog;

internal class NullScope : IDisposable
{
    public static NullScope Instance { get; } = new();

    public void Dispose()
    {
    }
}