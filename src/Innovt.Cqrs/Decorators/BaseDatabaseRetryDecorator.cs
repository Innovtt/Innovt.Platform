﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using System;
using System.Data.SqlClient;
using Innovt.Core.CrossCutting.Log;
using Polly;
using Polly.Retry;

namespace Innovt.Cqrs.Decorators;

/// <summary>
///     Provides a base class for implementing retry logic for database operations.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="BaseDatabaseRetryDecorator" /> class.
/// </remarks>
/// <param name="logger">The logger for capturing retry attempts.</param>
/// <param name="retryCount">The number of retry attempts (default is 3).</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="logger" /> is null.</exception>
public abstract class BaseDatabaseRetryDecorator(ILogger logger, int retryCount = 3)
{
    private readonly ILogger logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly int retryCount = retryCount;

    /// <summary>
    ///     Creates a resiliency log action for retry attempts.
    /// </summary>
    /// <returns>A log action for retry attempts.</returns>
    protected Action<Exception, TimeSpan, int, Context> LogResiliencyRetry()
    {
        return (exception, timeSpan, retryCount, context) =>
        {
            logger.Warning(
                $"Retry {retryCount} implemented of {context.PolicyKey} at {context.OperationKey} due to {exception}");
        };
    }

    /// <summary>
    ///     Creates an asynchronous retry policy for handling retry attempts.
    /// </summary>
    /// <returns>An asynchronous retry policy.</returns>
    protected virtual AsyncRetryPolicy CreateAsyncPolicy()
    {
        var policy = Policy.Handle<SqlException>().WaitAndRetryAsync(retryCount,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), LogResiliencyRetry());

        return policy;
    }

    /// <summary>
    ///     Creates a retry policy for handling retry attempts.
    /// </summary>
    /// <returns>A retry policy.</returns>
    protected virtual RetryPolicy CreatePolicy()
    {
        var policy = Policy.Handle<SqlException>().WaitAndRetry(retryCount,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), LogResiliencyRetry());

        return policy;
    }
}