// Innovt Company
// Author: Michel Borges
// Project: Innovt.Job.Core

using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.CrossCutting.Log;
using Timer = System.Timers.Timer;

namespace Innovt.Job.Core;

/// <summary>
/// Abstract base class for job implementations, providing common functionality for job management.
/// </summary>
public abstract class JobBase
{
    private readonly Timer heartBeat;
    protected readonly ILogger Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="JobBase"/> class.
    /// </summary>
    /// <param name="jobName">The name of the job.</param>
    /// <param name="logger">The logger to use for logging.</param>
    /// <param name="heartBeatIntervalInMiliSeconds">The interval for the heartbeat in milliseconds.</param>
    protected JobBase(string jobName, ILogger logger, double heartBeatIntervalInMiliSeconds)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));

        Name = jobName;
        heartBeat = new Timer(heartBeatIntervalInMiliSeconds)
        {
            Enabled = true,
            AutoReset = true
        };

        heartBeat.Elapsed += (sender, args) => OnHeartBeat();
    }

    /// <summary>
    /// Gets or sets the name of the job.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Event handler for the heartbeat.
    /// </summary>
    protected virtual void OnHeartBeat()
    {
        Logger.Info($"{Name}.HeartBeat");
    }

    /// <summary>
    /// Starts the job.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task Start()
    {
        Logger.Info($"Job [{Name}] starting at {DateTimeOffset.Now}");
        return OnStart();
    }

    /// <summary>
    /// Stops the job.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task Stop()
    {
        Logger.Info($"Job [{Name}] stopping at {DateTimeOffset.Now}");

        return OnStop();
    }

    /// <summary>
    /// Executes when the job is started.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnStart(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes when the job is stopped.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnStop(CancellationToken cancellationToken = default);
}