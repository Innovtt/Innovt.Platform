// Innovt Company
// Author: Michel Borges
// Project: Innovt.Job.Quartz

using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.CrossCutting.Log;
using Innovt.Job.Core;
using Quartz;

namespace Innovt.Job.Quartz;

/// <summary>
///     Abstract base class for Quartz job implementations, providing common functionality for scheduling and executing
///     jobs using Quartz framework.
/// </summary>
public abstract class QuartzJobBase : JobBase, IJob // where T : IJob
{
    private readonly int intervalInMinutes;
    private readonly JobKey key;
    private readonly IScheduler scheduler;
    private DateTimeOffset nextScheduleExecution = DateTimeOffset.MinValue;

    /// <summary>
    ///     Initializes a new instance of the <see cref="QuartzJobBase" /> class.
    /// </summary>
    /// <param name="name">The name of the job.</param>
    /// <param name="heartBeatInterval">The interval for the heartbeat in milliseconds.</param>
    /// <param name="logger">The logger to use for logging.</param>
    /// <param name="scheduler">The Quartz scheduler instance.</param>
    /// <param name="intervalInMinutes">The interval in minutes for job execution.</param>
    protected QuartzJobBase(string name, double heartBeatInterval, ILogger logger, IScheduler scheduler,
        int intervalInMinutes) : base(name, logger, heartBeatInterval)
    {
        this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        this.intervalInMinutes = intervalInMinutes;
        key = new JobKey(Name + "Key");
    }

    /// <summary>
    ///     Executes the Quartz job.
    /// </summary>
    /// <param name="context">The Quartz job execution context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task Execute(IJobExecutionContext context)
    {
        return OnExecute();
    }

    /// <summary>
    ///     Executes when the job is started.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override Task OnStart(CancellationToken cancellationToken = default)
    {
        return Schedule(cancellationToken);
    }

    /// <summary>
    ///     Executes when the job is stopped.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override Task OnStop(CancellationToken cancellationToken = default)
    {
        return scheduler.Shutdown(true, cancellationToken);
    }

    /// <summary>
    ///     Schedules the Quartz job for execution.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    ///     Executes the Quartz job logic.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnExecute();
}