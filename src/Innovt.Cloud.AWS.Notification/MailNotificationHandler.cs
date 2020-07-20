using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Innovt.Core.Utilities;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Notification.Core;
using Innovt.Notification.Core.Domain;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Notification
{
    public class MailNotificationHandler: AwsBaseService, INotificationHandler
    {
        public string DefaultCharset { get; set; } = "UTF-8";

        public MailNotificationHandler(ILogger logger) : base(logger)
        {
        }

        public MailNotificationHandler(ILogger logger,IAWSConfiguration configuration,string region=null) : base(logger,configuration,region)
        {

        }

        public async Task<dynamic> SendAsync(NotificationMessage message, CancellationToken cancellationToken = default)
        {
            Check.NotNull(message, nameof(message));
            Check.NotNull(message.Body, nameof(message.Body));
            Check.NotNull(message.To, nameof(message.To));
            Check.NotNull(message.From, nameof(message.From));
            Check.NotNull(message.From.Name, nameof(message.From.Name));
            Check.NotNull(message.From.Address, nameof(message.From.Address));
            Check.NotNull(message.Subject, nameof(message.Subject));

            //Invalid configuration set name<Não Responda>: only alphanumeric ASCII characters, '_', and '-' are allowed.
            var mailRequest = new SendEmailRequest()
            {
                Destination = new Destination(),
                Source      = $"{message.From.Name} <{message.From.Address}>",
                Message = new Message()
            };
               
            mailRequest.Message.Subject = new Content()
            {
                Charset = message.Subject.Charset.GetValueOrDefault(DefaultCharset),
                Data    = message.Subject.Content
            };
                
                
            mailRequest.Message.Body = new Body();
            if (message.Body.IsHtml)
            {
                mailRequest.Message.Body.Html = new Content()
                {
                    Charset = message.Body.Charset.GetValueOrDefault(DefaultCharset),
                    Data = message.Body.Content
                };
            }
            else
            {
                mailRequest.Message.Body.Text = new Content()
                {
                    Charset = message.Body.Charset.GetValueOrDefault(DefaultCharset),
                    Data = message.Body.Content
                };
            }
              
            mailRequest.Destination.ToAddresses = message.To.Select(a => $"{a.Name} <{a.Address}>").ToList();
             
            if (message.BccTo != null)
                mailRequest.Destination.BccAddresses = message.BccTo.Select(a => $"{a.Name} <{a.Address}>").ToList();

            if (message.CcTo != null)
                mailRequest.Destination.CcAddresses = message.CcTo.Select(a => $"{a.Name} <{a.Address}>").ToList();

            if (message.ReplyToAddresses != null)
                mailRequest.ReplyToAddresses = message.ReplyToAddresses.Select(a => $"{a.Name} <{a.Address}>").ToList();

            var policy = CreateDefaultRetryAsyncPolicy();

             using var simpleEmailClient = CreateService<AmazonSimpleEmailServiceClient>();

            var response = await policy.ExecuteAsync(async ()=> await simpleEmailClient.SendEmailAsync(mailRequest, cancellationToken));
               
            return response;
        }
    }
}