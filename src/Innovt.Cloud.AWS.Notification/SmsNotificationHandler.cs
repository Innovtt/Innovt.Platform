using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Innovt.Core.Utilities;
using System.Collections.Generic;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Notification.Core;
using Innovt.Notification.Core.Domain;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Notification
{
    public class SmsNotificationHandler : AwsBaseService, INotificationHandler
    {
        public SmsNotificationHandler(ILogger logger, IAWSConfiguration configuration) : base(logger, configuration)
        {
        }

        public SmsNotificationHandler(ILogger logger, IAWSConfiguration configuration, string region) : base(logger,
            configuration, region)
        {
        }

        private AmazonSimpleNotificationServiceClient _simpleNotificationClient;

        private AmazonSimpleNotificationServiceClient SimpleNotificationClient
        {
            get
            {
                if (_simpleNotificationClient == null)
                    _simpleNotificationClient = CreateService<AmazonSimpleNotificationServiceClient>();

                return _simpleNotificationClient;
            }
        }

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

        protected override void DisposeServices()
        {
            _simpleNotificationClient?.Dispose();
        }
    }
}