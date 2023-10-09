// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Notification

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;
using Innovt.Notification.Core;
using Innovt.Notification.Core.Domain;

namespace Innovt.Cloud.AWS.Notification;
/// <summary>
/// SMS notification handler using Amazon Simple Notification Service (SNS).
/// </summary>
public class SmsNotificationHandler : AwsBaseService, INotificationHandler
{
    private AmazonSimpleNotificationServiceClient _simpleNotificationClient;
    /// <summary>
    /// Initializes a new instance of the <see cref="SmsNotificationHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="configuration">The AWS configuration.</param>
    public SmsNotificationHandler(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="SmsNotificationHandler"/> class with a specific AWS region.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="configuration">The AWS configuration.</param>
    /// <param name="region">The AWS region.</param>
    public SmsNotificationHandler(ILogger logger, IAwsConfiguration configuration, string region) : base(logger,
        configuration, region)
    {
    }
    /// <summary>
    /// Gets the Amazon Simple Notification Service client.
    /// </summary>
    private AmazonSimpleNotificationServiceClient SimpleNotificationClient
    {
        get
        {
            if (_simpleNotificationClient == null)
                _simpleNotificationClient = CreateService<AmazonSimpleNotificationServiceClient>();

            return _simpleNotificationClient;
        }
    }
    /// <summary>
    /// Sends an SMS notification asynchronously.
    /// </summary>
    /// <param name="message">The notification message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A dynamic result representing the delivery status of the SMS.</returns>
    public async Task<dynamic> SendAsync(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        Check.NotNull(message, nameof(message));
        Check.NotNullWithBusinessException(message.Body, nameof(message.Body));
        Check.NotNullWithBusinessException(message.To, nameof(message.To));

        var deliveryResult = new List<dynamic>();

        var policy = base.CreateDefaultRetryAsyncPolicy();

        foreach (var to in message.To)
        {
            var request = new PublishRequest
            {
                Subject = message.Subject.Content,
                PhoneNumber = to.Address,
                Message = message.Body.Content
            };

            var result = await policy.ExecuteAsync(async () =>
                await SimpleNotificationClient.PublishAsync(request, cancellationToken));

            deliveryResult.Add(result);
        }

        return deliveryResult;
    }
    /// <summary>
    /// Disposes the resources used by the SMS notification handler.
    /// </summary>
    protected override void DisposeServices()
    {
        _simpleNotificationClient?.Dispose();
    }
}