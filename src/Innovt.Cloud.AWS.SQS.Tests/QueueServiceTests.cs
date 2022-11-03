using System.Threading;
using System.Threading.Tasks;
using Innovt.Cloud.AWS.Configuration;
using Innovt.CrossCutting.Log.Serilog;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.SQS.Tests
{
    public class QueueServiceTests
    {

        private QueueService<SimpleMessage> queueService;

        [SetUp]
        public void Setup()
        {
            queueService = new QueueService<SimpleMessage>(new Logger(), new DefaultAWSConfiguration("antecipa-dev"),"us-east-1", "SampleMichel");
            
        }

        [Test]
        [Ignore("Only for integrated tests")]
        public async Task Test1()
        {
            var result =    await queueService.EnQueueAsync(new SimpleMessage(), 0, CancellationToken.None);


            Assert.IsNotNull(result);   
        }
    }
}