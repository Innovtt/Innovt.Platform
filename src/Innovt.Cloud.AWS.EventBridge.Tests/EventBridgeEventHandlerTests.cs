using System.Diagnostics;
using System.Threading;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using NSubstitute;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.EventBridge.Tests;

public class EventBridgeEventHandlerTests
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
        var eventHandler = new EventBridgeEventHandler(busName, loggerMock, awsConfigurationMock, region);

        // Assert
        Assert.That(eventHandler, Is.Not.Null);
    }

    [Test]
    [Ignore("Integration Test")]
    public void Publish_Integrated()
    {
        var activitySource = new ActivitySource(nameof(EventBridgeEventHandlerTests));
        
        using var listener = new ActivityListener
        {
            ShouldListenTo = _ => true,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded
        };
        
        ActivitySource.AddActivityListener(listener);

        using var activity = activitySource.StartActivity("EventBridgeEventHandlerTests");

        var awsConfiguration = new DefaultAwsConfiguration("c2g-dev");

        // Act
        var eventHandler = new EventBridgeEventHandler("my-test-bus", loggerMock, awsConfiguration, "us-east-1");

        var sampleEvent = new UserConfirmedEvent();

        Assert.DoesNotThrowAsync(async () => await eventHandler.Publish(sampleEvent, CancellationToken.None));
    }
}