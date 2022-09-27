﻿// Innovt Company
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
public static class AsyncHelper
{
    private static readonly TaskFactory TaskFactory = new(CancellationToken.None,
        TaskCreationOptions.None,
        TaskContinuationOptions.None,
        TaskScheduler.Default);

    public static TResult RunSync<TResult>(Func<Task<TResult>> func, CancellationToken cancellationToken = default)
    {
        return TaskFactory
            .StartNew(func, cancellationToken)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    public static void RunSync(Func<Task> func, CancellationToken cancellationToken = default)
    {
        TaskFactory
            .StartNew(func, cancellationToken)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }
}