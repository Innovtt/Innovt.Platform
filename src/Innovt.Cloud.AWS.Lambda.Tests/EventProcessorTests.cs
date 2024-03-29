// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Tests

using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Innovt.Core.CrossCutting.Log;
using NSubstitute;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Lambda.Tests;

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
        Assert.Throws<ArgumentNullException>(() => new CustomEventProcessor(null));
    }

    [Test]
    public async Task Process_Use_Default_Logger_When_CustomLogger_IsNull()
    {
        var lambdaContext = new TestLambdaContext
        {
            Logger = Substitute.For<ILambdaLogger>(),
            AwsRequestId = "e71f4399-5cc8-44e9-818f-d2864ad1cf52"
        };

        var function = new CustomEventProcessor();

        await function.Process(new Person(), lambdaContext);

        lambdaContext.Logger.Received().LogLine(Arg.Any<string>());
    }

    [Test]
    public async Task Process_Use_Default_Logger()
    {
        var lambdaContext = new TestLambdaContext { AwsRequestId = "e71f4399-5cc8-44e9-818f-d2864ad1cf52" };

        var loggerMock = Substitute.For<ILogger>();

        var function = new CustomEventProcessor(loggerMock);

        await function.Process(new Person(), lambdaContext);

        loggerMock.Received().Info(Arg.Any<string>());
    }

    [Test]
    public async Task Process_Calls_LoggError_When_Function_ThrowsException()
    {
        var lambdaContext = new TestLambdaContext { AwsRequestId = "e71f4399-5cc8-44e9-818f-d2864ad1cf52" };

        var loggerMock = Substitute.For<ILogger>();

        var function = new CustomEventProcessor(loggerMock);

        try
        {
            await function.Process(new Person { Name = "Exception" }, lambdaContext);
        }
        catch (Exception)
        {
        }

        loggerMock.Received().Error(Arg.Any<Exception>(), Arg.Any<string>());
    }
}