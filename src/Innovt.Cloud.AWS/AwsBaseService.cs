// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

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

namespace Innovt.Cloud.AWS
{
    public abstract class AwsBaseService : IDisposable
    {
        protected readonly IAWSConfiguration Configuration;

        private bool disposed;

        private AwsBaseService()
        {
            RetryCount = 3;
            CircuitBreakerAllowedExceptions = 3;
            CircuitBreakerDurationOfBreak = TimeSpan.FromSeconds(5);
        }

        protected AwsBaseService(ILogger logger, IAWSConfiguration configuration) : this()
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected AwsBaseService(ILogger logger, IAWSConfiguration configuration, string region) : this()
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Region = region ?? throw new ArgumentNullException(nameof(region));
        }

        /// <summary>
        ///     This is the service region
        /// </summary>
        private string Region { get; }

        public int RetryCount { get; set; }

        protected int CircuitBreakerAllowedExceptions { get; set; }

        protected TimeSpan CircuitBreakerDurationOfBreak { get; set; }

        protected ILogger Logger { get; }

        public void Dispose()
        {
            Dispose(true);
        }


        protected RegionEndpoint GetServiceRegionEndPoint()
        {
            var region = Region ?? Configuration?.Region;

            if (region == null)
                return null;

            return RegionEndpoint.GetBySystemName(region);
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
                    : (T) Activator.CreateInstance(typeof(T), serviceRegion);


            return serviceRegion == null
                ? (T) Activator.CreateInstance(typeof(T), credentials)
                : (T) Activator.CreateInstance(typeof(T), credentials, serviceRegion);
        }


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
                r.StatusCode == HttpStatusCode.ServiceUnavailable ||
                r.StatusCode == HttpStatusCode.InternalServerError).WaitAndRetryAsync(RetryCount, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), LogResiliencyRetry());
        }

        protected virtual RetryPolicy CreateDefaultRetryPolicy()
        {
            return Policy.Handle<AmazonServiceException>(r =>
                r.StatusCode == HttpStatusCode.ServiceUnavailable ||
                r.StatusCode == HttpStatusCode.InternalServerError).WaitAndRetry(RetryCount, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), LogResiliencyRetry());
        }

        protected virtual AsyncRetryPolicy CreateRetryAsyncPolicy<T>() where T : Exception
        {
            return Policy.Handle<T>().WaitAndRetryAsync(RetryCount, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), LogResiliencyRetry());
        }

        protected virtual AsyncRetryPolicy CreateRetryAsyncPolicy<T, T1>() where T : Exception where T1 : Exception
        {
            return Policy.Handle<T>().Or<T1>().WaitAndRetryAsync(RetryCount, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), LogResiliencyRetry());
        }

        protected virtual AsyncRetryPolicy CreateRetryAsyncPolicy<T, T1, T2>()
            where T : Exception where T1 : Exception where T2 : Exception
        {
            return Policy.Handle<T>().Or<T1>().Or<T2>()
                .WaitAndRetryAsync(RetryCount, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), LogResiliencyRetry());
        }

        protected virtual AsyncRetryPolicy CreateRetryAsyncPolicy<T, T1, T2, T3>() where T : Exception
            where T1 : Exception
            where T2 : Exception
            where T3 : Exception
        {
            return Policy.Handle<T>().Or<T1>().Or<T2>().Or<T3>()
                .WaitAndRetryAsync(RetryCount, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), LogResiliencyRetry());
        }

        protected virtual AsyncRetryPolicy CreateRetryAsyncPolicy<T, T1, T2, T3, T4>() where T : Exception
            where T1 : Exception
            where T2 : Exception
            where T3 : Exception
            where T4 : Exception
        {
            return Policy.Handle<T>().Or<T1>().Or<T2>().Or<T3>().Or<T4>()
                .WaitAndRetryAsync(RetryCount, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), LogResiliencyRetry());
        }

        protected virtual AsyncCircuitBreakerPolicy CreateCircuitBreaker<T, T1>()
            where T : Exception where T1 : Exception
        {
            return Policy.Handle<T>()
                .CircuitBreakerAsync(CircuitBreakerAllowedExceptions, CircuitBreakerDurationOfBreak);
        }

        private void Dispose(bool disposing)
        {
            if (disposed || !disposing)
                return;

            DisposeServices();

            disposed = true;
        }

        ~AwsBaseService()
        {
            Dispose(false);
        }

        protected abstract void DisposeServices();
    }
}