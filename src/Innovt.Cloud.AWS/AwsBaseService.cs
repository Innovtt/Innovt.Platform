// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS

using System;
using System.Net;
using Amazon;
using Amazon.Runtime;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RetryPolicy = Polly.Retry.RetryPolicy;

namespace Innovt.Cloud.AWS;

/// <summary>
///     An abstract base class for AWS services with common functionality for managing AWS service configurations, retries,
///     and circuit breakers.
/// </summary>
[CLSCompliant(false)]
public abstract class AwsBaseService : IDisposable
{
    protected IAwsConfiguration Configuration { get; }

    private bool disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AwsBaseService" /> class.
    /// </summary>
    private AwsBaseService()
    {
        RetryCount = 3;
        CircuitBreakerAllowedExceptions = 3;
        CircuitBreakerDurationOfBreak = TimeSpan.FromSeconds(5);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AwsBaseService" /> class with a logger and AWS configuration.
    /// </summary>
    /// <param name="logger">The logger for logging service activities.</param>
    /// <param name="configuration">The AWS configuration for the service.</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected AwsBaseService(ILogger logger, IAwsConfiguration configuration) : this()
    {
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AwsBaseService" /> class with a logger, AWS configuration, and AWS
    ///     region.
    /// </summary>
    /// <param name="logger">The logger for logging service activities.</param>
    /// <param name="configuration">The AWS configuration for the service.</param>
    /// <param name="region">The AWS region for the service.</param>
    protected AwsBaseService(ILogger logger, IAwsConfiguration configuration, string region) : this()
    {
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Region = region ?? throw new ArgumentNullException(nameof(region));
    }

    /// <summary>
    ///     This is the service region
    /// </summary>
    private string Region { get; }

    /// <summary>
    ///     Gets or sets the number of retry attempts for AWS service calls.
    /// </summary>
    private int RetryCount { get; }

    /// <summary>
    ///     It represents the exponential backoffice in seconds
    /// </summary>
    public int ExponentialBackoffInSeconds { get; set; } = 1;

    /// <summary>
    ///     Gets or sets the number of allowed exceptions before the circuit breaker opens.
    /// </summary>
    private int CircuitBreakerAllowedExceptions { get; }

    /// <summary>
    ///     Gets or sets the duration of the circuit breaker break when it opens.
    /// </summary>
    private TimeSpan CircuitBreakerDurationOfBreak { get; }

    /// <summary>
    ///     Gets the logger for logging service activities.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    ///     Disposes of the resources used by the service.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Gets the AWS service region endpoint based on the configured region.
    /// </summary>
    /// <returns>The AWS service region endpoint.</returns>
    protected RegionEndpoint GetServiceRegionEndPoint()
    {
        var region = Region ?? Configuration?.Region;

        return region == null ? null : RegionEndpoint.GetBySystemName(region);
    }

    /// <summary>
    ///     This method will decide about Configuration or Profile AWS Services
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected T CreateService<T>() where T : IAmazonService
    {
        var credentials = Configuration.GetCredential();
        var serviceRegion = GetServiceRegionEndPoint();

        if (credentials == null)
            return serviceRegion == null
                ? Activator.CreateInstance<T>()
                : (T)Activator.CreateInstance(typeof(T), serviceRegion);


        return serviceRegion == null
            ? (T)Activator.CreateInstance(typeof(T), credentials)
            : (T)Activator.CreateInstance(typeof(T), credentials, serviceRegion);
    }

    /// <summary>
    ///     Generates an action to record replay attempt information for resiliency purposes.
    /// </summary>
    /// <returns>An action with a logger call.</returns>
    private Action<Exception, TimeSpan, int, Context> LogResiliencyRetry()
    {
        return (exception, timeSpan, retryCount, context) =>
        {
            Logger.Warning(
                $"Retry {retryCount} implemented of {context.PolicyKey} at {context.OperationKey} due to {exception}");
        };
    }

    /// <summary>
    ///     Basic Retry Policy using AmazonServiceException
    /// </summary>
    /// <returns></returns>
    protected virtual AsyncRetryPolicy CreateDefaultRetryAsyncPolicy()
    {
        return Policy.Handle<AmazonServiceException>(r =>
            r.StatusCode is HttpStatusCode.ServiceUnavailable or HttpStatusCode.InternalServerError).WaitAndRetryAsync(
            RetryCount, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(ExponentialBackoffInSeconds, retryAttempt)), LogResiliencyRetry());
    }

    /// <summary>
    ///     Creates a default retry policy for handling AmazonServiceException exceptions
    ///     with specific HTTP status codes(ServiceUnavailable or InternalServerError).
    /// </summary>
    protected virtual RetryPolicy CreateDefaultRetryPolicy()
    {
        return Policy.Handle<AmazonServiceException>(r =>
            r.StatusCode is HttpStatusCode.ServiceUnavailable or HttpStatusCode.InternalServerError).WaitAndRetry(
            RetryCount, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(ExponentialBackoffInSeconds, retryAttempt)), LogResiliencyRetry());
    }

    /// <summary>
    ///     Creates an asynchronous retry policy for handling exceptions of type T.
    /// </summary>
    /// <typeparam name="T">The type of exception to handle.</typeparam>
    /// <returns>An asynchronous retry policy.</returns>
    protected virtual AsyncRetryPolicy CreateRetryAsyncPolicy<T>() where T : Exception
    {
        return Policy.Handle<T>().WaitAndRetryAsync(RetryCount, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(ExponentialBackoffInSeconds, retryAttempt)), LogResiliencyRetry());
    }

    /// <summary>
    ///     Creates an asynchronous retry policy for handling exceptions of type T and T1.
    /// </summary>
    /// <typeparam name="T">The first type of exception to handle.</typeparam>
    /// <typeparam name="T1">The second type of exception to handle.</typeparam>
    /// <returns>An asynchronous retry policy.</returns>
    protected virtual AsyncRetryPolicy CreateRetryAsyncPolicy<T, T1>() where T : Exception where T1 : Exception
    {
        return Policy.Handle<T>().Or<T1>().WaitAndRetryAsync(RetryCount, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(ExponentialBackoffInSeconds, retryAttempt)), LogResiliencyRetry());
    }

    protected virtual AsyncRetryPolicy CreateRetryAsyncPolicy<T, T1, T2>()
        where T : Exception where T1 : Exception where T2 : Exception
    {
        return Policy.Handle<T>().Or<T1>().Or<T2>()
            .WaitAndRetryAsync(RetryCount, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(ExponentialBackoffInSeconds, retryAttempt)), LogResiliencyRetry());
    }

    protected virtual AsyncRetryPolicy CreateRetryAsyncPolicy<T, T1, T2, T3>() where T : Exception
        where T1 : Exception
        where T2 : Exception
        where T3 : Exception
    {
        return Policy.Handle<T>().Or<T1>().Or<T2>().Or<T3>()
            .WaitAndRetryAsync(RetryCount, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(ExponentialBackoffInSeconds, retryAttempt)), LogResiliencyRetry());
    }

    protected virtual AsyncRetryPolicy CreateRetryAsyncPolicy<T, T1, T2, T3, T4>() where T : Exception
        where T1 : Exception
        where T2 : Exception
        where T3 : Exception
        where T4 : Exception
    {
        return Policy.Handle<T>().Or<T1>().Or<T2>().Or<T3>().Or<T4>()
            .WaitAndRetryAsync(RetryCount, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(ExponentialBackoffInSeconds, retryAttempt)), LogResiliencyRetry());
    }

    protected virtual AsyncCircuitBreakerPolicy CreateCircuitBreaker<T, T1>()
        where T : Exception where T1 : Exception
    {
        return Policy.Handle<T>()
            .CircuitBreakerAsync(CircuitBreakerAllowedExceptions, CircuitBreakerDurationOfBreak);
    }

    /// <summary>
    ///     Disposes of resources used by the AwsBaseService.
    /// </summary>
    /// <param name="disposing">True if called from the Dispose method; false if called from the finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposed || !disposing)
            return;

        DisposeServices();

        disposed = true;
    }

    /// <summary>
    ///     Finalizer for AwsBaseService.
    /// </summary>
    ~AwsBaseService()
    {
        Dispose(false);
    }

    /// <summary>
    ///     Disposes of any services used by the AwsBaseService.
    /// </summary>
    protected abstract void DisposeServices();
}