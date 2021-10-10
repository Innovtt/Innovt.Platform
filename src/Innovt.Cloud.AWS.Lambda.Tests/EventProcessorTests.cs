using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Innovt.Core.CrossCutting.Log;
using Innovt.CrossCutting.Log.Serilog;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Lambda.Tests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Process_Throws_Exception_When_CustomLogger_IsNull()
        {
            Assert.Throws<ArgumentNullException>(()=> new CustomEventProcessor(null));
        }

        [Test]
        public async Task Process_Use_Default_Logger_When_CustomLogger_IsNull()
        {
            var lambdaContext = new TestLambdaContext()
            {
                Logger = Substitute.For<ILambdaLogger>()
            };

            var function = new CustomEventProcessor();   

            await function.Process(new Person(), lambdaContext);

            lambdaContext.Logger.Received().LogLine(Arg.Any<string>());            
        }

        [Test]
        public async Task Process_Use_Default_Logger()
        {
            var lambdaContext = new TestLambdaContext();

            var loggerMock = Substitute.For<ILogger>();

            var function = new CustomEventProcessor(loggerMock);

            await function.Process(new Person(), lambdaContext);

            loggerMock.Received().Info(Arg.Any<string>());
        }

        [Test]
        public async Task Process_Calls_LoggError_When_Function_ThrowsException()
        {
            var lambdaContext = new TestLambdaContext();

            var loggerMock = Substitute.For<ILogger>();

            var function = new CustomEventProcessor(loggerMock);

            try
            {
                await function.Process(new Person() { Name="Exception" }, lambdaContext);
            }
            catch (Exception)
            {                
            }

            loggerMock.Received().Error(Arg.Any<Exception>(), Arg.Any<string>());
        }
    }
}