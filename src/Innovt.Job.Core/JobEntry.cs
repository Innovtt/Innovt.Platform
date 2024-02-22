// Innovt Company
// Author: Michel Borges
// Project: Innovt.Job.Core

using System;
using System.IO;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
using Microsoft.Extensions.Configuration;

namespace Innovt.Job.Core;

/// <summary>
///     Abstract base class for job entry implementations, providing common functionality for setting up and running jobs.
/// </summary>
public abstract class JobEntry
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="JobEntry" /> class.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <param name="jobName">The name of the job.</param>
    protected JobEntry(string[] args, string jobName)
    {
        JobName = jobName;
    }

    /// <summary>
    ///     Gets the name of the job.
    /// </summary>
    public string JobName { get; }

    /// <summary>
    ///     Gets or sets the configuration.
    /// </summary>
    public IConfiguration Configuration { get; protected set; }

    /// <summary>
    ///     Sets up the configuration using appsettings.json.
    /// </summary>
    protected virtual void SetupConfiguration()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true);

        Configuration = builder.Build();
    }


    /// <summary>
    ///     Sets up the IoC container.
    /// </summary>
    private void SetupContainer()
    {
        var container = CreateIocContainer();

        container.CheckConfiguration();

        IocLocator.Initialize(container);
    }

    /// <summary>
    ///     Creates the IoC container.
    /// </summary>
    /// <returns>The IoC container.</returns>
    protected abstract IContainer CreateIocContainer();

    /// <summary>
    ///     Creates the job instance.
    /// </summary>
    /// <returns>The job instance.</returns>
    protected abstract JobBase CreateJob();

    /// <summary>
    ///     Runs the job.
    /// </summary>
    public void Run()

    {
        Console.WriteLine($"************** Starting  {JobName} Job  **************");
        try
        {
            Console.WriteLine("************** Checking the ConfigurationFile **************");
            SetupConfiguration();
            Console.WriteLine("************** ConfigurationFile  DONE! **************");

            Console.WriteLine("************** SetupContainer **************");
            SetupContainer();
            Console.WriteLine("************** SetupContainer  DONE! **************");

            var job = CreateJob();

            if (job == null)
                throw new CriticalException($"The call for CreateJob method return NULL for Job Name {JobName}");


            AsyncHelper.RunSync(async () => await job.Start().ConfigureAwait(false));
        }
        catch (Exception)
        {
            Console.WriteLine($"************** Execption Ocurred at Job {JobName}  **************");
            throw;
        }
    }
}