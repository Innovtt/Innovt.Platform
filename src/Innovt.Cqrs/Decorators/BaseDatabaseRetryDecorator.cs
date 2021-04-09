using Innovt.Core.CrossCutting.Log;
using Polly;
using Polly.Retry;
using System;
using System.Data.SqlClient;

namespace Innovt.Cqrs.Decorators
{
    public abstract class BaseDatabaseRetryDecorator
    {
        private readonly ILogger logger;
        private readonly int retryCount;

        protected BaseDatabaseRetryDecorator(ILogger logger, int retryCount = 3)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.retryCount = retryCount;
        }

        protected Action<Exception, TimeSpan, int, Context> LogResiliencyRetry()
        {
            return (exception, timeSpan, retryCount, context) =>
            {
                logger.Warning(
                    $"Retry {retryCount} implemented of {context.PolicyKey} at {context.OperationKey} due to {exception}");
            };
        }

        protected virtual AsyncRetryPolicy CreateAsyncPolicy()
        {
            var policy = Policy.Handle<SqlException>().WaitAndRetryAsync(retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), LogResiliencyRetry());

            return policy;
        }

        protected virtual RetryPolicy CreatePolicy()
        {
            var policy = Policy.Handle<SqlException>().WaitAndRetry(retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), LogResiliencyRetry());

            return policy;
        }
    }
}