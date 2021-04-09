using Innovt.Core.CrossCutting.Log;
using Innovt.Job.Core;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Job.Quartz
{
    public abstract class QuartzJobBase : JobBase, IJob // where T : IJob
    {
        private readonly IScheduler scheduler;
        private readonly JobKey key;
        private readonly int intervalInMinutes;
        private DateTimeOffset nextScheduleExecution = DateTimeOffset.MinValue;

        protected QuartzJobBase(string name, double heartBeatInterval, ILogger logger, IScheduler scheduler,
            int intervalInMinutes) : base(name, logger, heartBeatInterval)
        {
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            this.intervalInMinutes = intervalInMinutes;
            key = new JobKey(Name + "Key");
        }

        protected override async Task OnStart(CancellationToken cancellationToken = default)
        {
            await Schedule(cancellationToken);
        }

        protected override async Task OnStop(CancellationToken cancellationToken = default)
        {
            await scheduler.Shutdown(true, cancellationToken);
        }

        public virtual async Task Schedule(CancellationToken cancellationToken = default)
        {
            //first execution
            if (nextScheduleExecution == DateTimeOffset.MinValue)
            {
                nextScheduleExecution = DateTimeOffset.Now;
                await scheduler.Start(cancellationToken);
            }
            else
            {
                var jobExist = await scheduler.CheckExists(key, cancellationToken);

                if (jobExist)
                {
                    nextScheduleExecution = DateTime.Now.AddMinutes(intervalInMinutes);
                    await scheduler.DeleteJob(key, cancellationToken);
                }

                var trigger = TriggerBuilder.Create().WithIdentity(key.Name).StartAt(nextScheduleExecution).Build();

                var job = JobBuilder.Create().WithIdentity(key).Build();

                await scheduler.ScheduleJob(job, trigger, cancellationToken);
            }
        }

        protected abstract Task OnExecute();

        public Task Execute(IJobExecutionContext context)
        {
            return OnExecute();
        }
    }
}