using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Lambda.Tests
{
    public class CustomEventProcessor : EventProcessor<Person>
    {

        public CustomEventProcessor(ILogger logger):base(logger)
        {
        }
        public CustomEventProcessor() : base()
        {
        }
        protected override Task Handle(Person message, ILambdaContext context)
        {
            //for test
            if (message.Name == "Exception")
                throw new Exception("Exception");


            return Task.CompletedTask;
        }

        protected override IContainer SetupIocContainer()
        {
            return null;            
        }
    }
}
