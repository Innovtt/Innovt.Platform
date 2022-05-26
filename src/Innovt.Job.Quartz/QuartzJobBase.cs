// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Job.Quartz
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.CrossCutting.Log;
using Innovt.Job.Core;
using Quartz;

namespace Innovt.Job.Quartz;

public abstract class QuartzJobBase : JobBase, IJob // where T : IJob
{
    private readonly int intervalInMinutes;
    private readonly JobKey key;
    private readonly IScheduler scheduler;
    private DateTimeOffset nextScheduleExecution = DateTimeOffset.MinValue;

    protected QuartzJobBase(string name, double heartBeatInterval, ILogger logger, IScheduler scheduler,
        int intervalInMinutes) : base(name, logger, heartBeatInterval)
    {
        this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        this.intervalInMinutes = intervalInMinutes;
        key = new JobKey(Name + "Key");
    }

    public Task Execute(IJobExecutionContext context)
    {
        return OnExecute();
    }

    protected override Task OnStart(CancellationToken cancellationToken = default)
    {
        return Schedule(cancellationToken);
    }

    protected override Task OnStop(CancellationToken cancellationToken = default)
    {
        return scheduler.Shutdown(true, cancellationToken);
    }

    public virtual async Task Schedule(CancellationToken cancellationToken = default)
    {
        //first execution
        if (nextScheduleExecution == DateTimeOffset.MinValue)
        {
            nextScheduleExecution = DateTimeOffset.Now;
            await scheduler.Start(cancellationToken).ConfigureAwait(false);
        }
        else
        {
            var jobExist = await scheduler.CheckExists(key, cancellationToken).ConfigureAwait(false);

            if (jobExist)
            {
                nextScheduleExecution = DateTime.Now.AddMinutes(intervalInMinutes);
                await scheduler.DeleteJob(key, cancellationToken).ConfigureAwait(false);
            }

            var trigger = TriggerBuilder.Create().WithIdentity(key.Name).StartAt(nextScheduleExecution).Build();

            var job = JobBuilder.Create().WithIdentity(key).Build();

            await scheduler.ScheduleJob(job, trigger, cancellationToken).ConfigureAwait(false);
        }
    }

    protected abstract Task OnExecute();
}