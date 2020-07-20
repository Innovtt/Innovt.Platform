using Amazon;
using Amazon.Runtime;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System;
using System.Net;
using RetryPolicy = Polly.Retry.RetryPolicy;

namespace Innovt.Cloud.AWS
{
    public abstract class AwsBaseService
    {
        protected readonly IAWSConfiguration Configuration;

        private string region;
        
        public RegionEndpoint Region { get; set; }

        public int RetryCount { get; set; }

        public int CircuitBreakerAllowedExceptions { get; set; }

        public TimeSpan CircuitBreakerDurationOfBreak { get; set; }

        public ILogger Logger { get; }


        protected AwsBaseService(ILogger logger, IAWSConfiguration configuration, string region):this()
        {
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.region = region ?? throw new ArgumentNullException(nameof(region));
        }

        protected AwsBaseService(ILogger logger):this()
        {  
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

       

        protected AwsBaseService()
        { 
            RetryCount = 3;
            CircuitBreakerAllowedExceptions = 3;
            CircuitBreakerDurationOfBreak = TimeSpan.FromSeconds(5);
        }

        protected RegionEndpoint GetRegionEndPoint(string region)
        {
            if (string.IsNullOrEmpty(region))
                region = this.Configuration?.Region;

            if (string.IsNullOrEmpty(region))
                throw new ConfigurationException("AWS Region name not defined for this service.");

            var awsRegion = RegionEndpoint.GetBySystemName(region);

            if (awsRegion == null)
                throw new ConfigurationException($"Invalid AWS Region {region}.");

            return awsRegion;
        }
        
        /// <summary>
        /// This method will decide about Configuration or Profile AWS Services 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T CreateService<T>() where T: IAmazonService
        {
            if (Configuration == null)
            {
                return Activator.CreateInstance<T>();
            }


     
            var instance = (T)Activator.CreateInstance(typeof(T), Configuration.AccessKey,Configuration.SecretKey, Region);

            return instance;
        }


        private Action<Exception, TimeSpan, int, Context> LogResiliencyRetry()
        {
            return (exception, timeSpan, retryCount, context) =>
            {
                Logger.Warning($"Retry {retryCount} implemented of {context.PolicyKey} at {context.OperationKey} due to {exception}");
            };
        }

        /// <summary>
        /// Basic Retry Policy using AmazonServiceException
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

        protected virtual AsyncRetryPolicy CreateRetryAsyncPolicy<T>() where T: Exception
        {       
           return Policy.Handle<T>().WaitAndRetryAsync(RetryCount, retryAttempt => 
	                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), LogResiliencyRetry() );

        }

        protected virtual AsyncRetryPolicy CreateRetryAsyncPolicy<T,T1>() where T: Exception where T1: Exception
        {       
           return Policy.Handle<T>().Or<T1>().WaitAndRetryAsync(RetryCount, retryAttempt => 
	                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), LogResiliencyRetry());

        }

        protected virtual AsyncCircuitBreakerPolicy CreateCircuitBreaker<T,T1>() where T: Exception where T1: Exception
        {       
           return Policy.Handle<T>().CircuitBreakerAsync(CircuitBreakerAllowedExceptions, CircuitBreakerDurationOfBreak);
        }

    }
}
