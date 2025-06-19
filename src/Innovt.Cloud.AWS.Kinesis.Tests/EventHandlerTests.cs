using System.Threading;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using NSubstitute;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Kinesis.Tests;

public class EventHandlerTests
{
     
    [SetUp]
    public void TearUp()
    {
        loggerMock = Substitute.For<ILogger>();
        awsConfigurationMock = Substitute.For<IAwsConfiguration>();
    }
    

    private ILogger loggerMock;
    private IAwsConfiguration awsConfigurationMock;
    
    [Test]
    public void EventHandler_Constructor_ShouldInitializeWithValidParameters()
    {
        // Arrange
        var busName = "test-bus";
        var region = "us-west-2";

        // Act
        var eventHandler = new EventHandler(busName, loggerMock, awsConfigurationMock, region);

        // Assert
        Assert.That(eventHandler, Is.Not.Null);
    }
    
    [Test]
    [Ignore("Integration Test")]
    public void Publish_Integrated()
    {
        var awsConfiguration = new DefaultAwsConfiguration("c2g-dev");
        
        // Act
        var eventHandler = new EventHandler("EventStream", loggerMock, awsConfiguration, "us-east-1");

        var sampleEvent = new UserConfirmedEvent();

    
        Assert.DoesNotThrowAsync(async () => await eventHandler.Publish(sampleEvent, CancellationToken.None));
    }

}