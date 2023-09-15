// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Threading;
using System.Threading.Tasks;

[assembly: CLSCompliant(false)]

namespace Innovt.Core.Utilities;

/// <summary>
///     Reference https://cpratt.co/async-tips-tricks/
/// </summary>

/// <summary>
/// Provides helper methods for synchronously executing asynchronous tasks.
/// </summary>
public static class AsyncHelper {
    private static readonly TaskFactory TaskFactory = new TaskFactory(
        CancellationToken.None,
        TaskCreationOptions.None,
        TaskContinuationOptions.None,
        TaskScheduler.Default);

    /// <summary>
    /// Runs an asynchronous function synchronously and returns its result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the asynchronous function.</typeparam>
    /// <param name="func">The asynchronous function to run synchronously.</param>
    /// <param name="cancellationToken">An optional cancellation token for the operation.</param>
    /// <returns>The result of the asynchronous function.</returns>
    public static TResult RunSync<TResult>(Func<Task<TResult>> func, CancellationToken cancellationToken = default) {
        return TaskFactory
            .StartNew(func, cancellationToken)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    /// <summary>
    /// Runs an asynchronous action synchronously.
    /// </summary>
    /// <param name="func">The asynchronous action to run synchronously.</param>
    /// <param name="cancellationToken">An optional cancellation token for the operation.</param>
    public static void RunSync(Func<Task> func, CancellationToken cancellationToken = default) {
        TaskFactory
            .StartNew(func, cancellationToken)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }
}