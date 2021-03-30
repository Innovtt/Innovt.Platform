using System;
using System.IO;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
using Microsoft.Extensions.Configuration;

namespace Innovt.Job.Core
{
    public abstract class JobEntry
    {
        public string JobName { get; }
        public IConfiguration Configuration { get; protected set; }

        protected JobEntry(string[] args, string jobName)
        {
            JobName = jobName;
        }

        protected virtual void SetupConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

            Configuration = builder.Build();
        }


        private void SetupContainer()
        {
            var container = CreateIocContainer();

            container.CheckConfiguration();

            IOCLocator.Initialize(container);
        }

        protected abstract IContainer CreateIocContainer();
        protected abstract JobBase CreateJob();


        public void Run()

        {
            Console.WriteLine($"************** Starting  {JobName} Job  **************");
            try
            {
                Console.WriteLine($"************** Checking the ConfigurationFile **************");
                SetupConfiguration();
                Console.WriteLine($"************** ConfigurationFile  DONE! **************");

                Console.WriteLine($"************** SetupContainer **************");
                SetupContainer();
                Console.WriteLine($"************** SetupContainer  DONE! **************");

                var job = CreateJob();

                if (job == null)
                    throw new CriticalException($"The call for CreateJob method return NULL for Job Name {JobName}");


                AsyncHelper.RunSync(async () => await job.Start());
            }
            catch (Exception)
            {
                Console.WriteLine($"************** Execption Ocurred at Job {JobName}  **************");
                throw;
            }
        }
    }
}