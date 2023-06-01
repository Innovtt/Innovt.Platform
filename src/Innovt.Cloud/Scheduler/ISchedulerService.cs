using System;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.Scheduler
{
    public interface ISchedulerService
    {
        Task<string> ScheduleQueueMessageAsync<TK>(TK message, string queueName, DateTime dateTime, string scheduleName, string scheduleGroupName = null, int maximumRetryAttempts = 3, CancellationToken cancellationToken = default);

        Task<string> ScheduleQueueMessageAsync<TK>(TK message, string queueName, string cronExpression, string scheduleName, string scheduleGroupName = null, int maximumRetryAttempts = 3, CancellationToken cancellationToken = default);

        Task DeleteSchedulerAsync(string scheduleName, string scheduleGroupName = null, CancellationToken cancellationToken = default);
    }
}