// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Notification
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;
using Innovt.Notification.Core;
using Innovt.Notification.Core.Domain;

namespace Innovt.Cloud.AWS.Notification
{
    public class MailNotificationHandler : AwsBaseService, INotificationHandler
    {
        private AmazonSimpleEmailServiceClient _simpleEmailClient;

        public MailNotificationHandler(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
        {
        }

        public MailNotificationHandler(ILogger logger, IAwsConfiguration configuration, string region) : base(logger,
            configuration, region)
        {
        }

        public string DefaultCharset { get; set; } = "UTF-8";

        private AmazonSimpleEmailServiceClient SimpleEmailClient
        {
            get
            {
                if (_simpleEmailClient == null) _simpleEmailClient = CreateService<AmazonSimpleEmailServiceClient>();

                return _simpleEmailClient;
            }
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
            var mailRequest = new SendEmailRequest
            {
                Destination = new Destination(),
                Source = $"{message.From.Name} <{message.From.Address}>",
                Message = new Message()
            };

            mailRequest.Message.Subject = new Content
            {
                Charset = message.Subject.Charset.GetValueOrDefault(DefaultCharset),
                Data = message.Subject.Content
            };


            mailRequest.Message.Body = new Body();
            if (message.Body.IsHtml)
                mailRequest.Message.Body.Html = new Content
                {
                    Charset = message.Body.Charset.GetValueOrDefault(DefaultCharset),
                    Data = message.Body.Content
                };
            else
                mailRequest.Message.Body.Text = new Content
                {
                    Charset = message.Body.Charset.GetValueOrDefault(DefaultCharset),
                    Data = message.Body.Content
                };

            mailRequest.Destination.ToAddresses = message.To.Select(a => $"{a.Name} <{a.Address}>").ToList();

            if (message.BccTo != null)
                mailRequest.Destination.BccAddresses = message.BccTo.Select(a => $"{a.Name} <{a.Address}>").ToList();

            if (message.CcTo != null)
                mailRequest.Destination.CcAddresses = message.CcTo.Select(a => $"{a.Name} <{a.Address}>").ToList();

            if (message.ReplyToAddresses != null)
                mailRequest.ReplyToAddresses = message.ReplyToAddresses.Select(a => $"{a.Name} <{a.Address}>").ToList();

            var policy = CreateDefaultRetryAsyncPolicy();

            var response = await policy.ExecuteAsync(async () =>
                await SimpleEmailClient.SendEmailAsync(mailRequest, cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);

            return response;
        }

        protected override void DisposeServices()
        {
            _simpleEmailClient?.Dispose();
        }
    }
}