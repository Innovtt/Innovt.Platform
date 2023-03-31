using System;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.Scheduler
{
    public interface ISchedulerService
    {
        Task<string> ScheduleQueueMessageAsync<TK>(TK message, string queueName, DateTime dateTime, string scheduleName,
                CancellationToken cancellationToken = default);
    }
}