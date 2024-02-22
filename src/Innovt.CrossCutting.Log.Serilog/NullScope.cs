// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.Log.Serilog

using System;

namespace Innovt.CrossCutting.Log.Serilog;

/// <summary>
///     Represents a null scope for logging purposes where no scope is applied.
/// </summary>
internal class NullScope : IDisposable
{
    /// <summary>
    ///     Gets the singleton instance of the null scope.
    /// </summary>
    public static NullScope Instance { get; } = new();

    /// <summary>
    ///     Disposes the null scope.
    /// </summary>
    public void Dispose()
    {
    }
}