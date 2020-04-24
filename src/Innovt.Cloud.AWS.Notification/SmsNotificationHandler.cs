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
    public class SmsNotificationHandler: AWSBaseService, INotificationHandler
    {

        public SmsNotificationHandler(ILogger logger) : base(logger)
        {
        }


        public SmsNotificationHandler(IAWSConfiguration configuration,ILogger logger,string region=null) : base(configuration,logger,region)
        {
        }

        public async Task<dynamic> SendAsync(NotificationMessage message, CancellationToken cancellationToken = default)
        {
            Check.NotNull(message,nameof(message));
            Check.NotNullWithBusinessException(message.Body, nameof(message.Body));
            Check.NotNullWithBusinessException(message.To, nameof(message.To));

            List<dynamic> deliveryResult = new List<dynamic>();
            
            var policy = base.CreateDefaultRetryAsyncPolicy();

            using var simpleNotificationClient = CreateService<AmazonSimpleNotificationServiceClient>();

            foreach(var to in message.To) {
                    var request = new PublishRequest
                    {
                        Subject = message.Subject.Content,
                        PhoneNumber = to.Address,
                        Message = message.Body.Content
                    };

                    var result = await policy.ExecuteAsync(async ()=> await  simpleNotificationClient.PublishAsync(request, cancellationToken));

                    deliveryResult.Add(result);
            }
            

            return deliveryResult;
        }
    }
}