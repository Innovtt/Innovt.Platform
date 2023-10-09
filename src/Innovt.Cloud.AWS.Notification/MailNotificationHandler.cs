// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Notification

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

namespace Innovt.Cloud.AWS.Notification;
/// <summary>
/// Handles sending notifications via email using Amazon Simple Email Service (SES).
/// </summary>
public class MailNotificationHandler : AwsBaseService, INotificationHandler
{
    private AmazonSimpleEmailServiceClient _simpleEmailClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="MailNotificationHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="configuration">The AWS configuration.</param>
    public MailNotificationHandler(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="MailNotificationHandler"/> class with a specified region.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="configuration">The AWS configuration.</param>
    /// <param name="region">The AWS region.</param>
    public MailNotificationHandler(ILogger logger, IAwsConfiguration configuration, string region) : base(logger,
        configuration, region)
    {
    }
    /// <summary>
    /// Gets or sets the default charset for the email.
    /// </summary>
    public string DefaultCharset { get; set; } = "UTF-8";

    /// <summary>
    /// Gets the Amazon Simple Email Service (SES) client instance.
    /// </summary>
    private AmazonSimpleEmailServiceClient SimpleEmailClient
    {
        get
        {
            if (_simpleEmailClient == null) _simpleEmailClient = CreateService<AmazonSimpleEmailServiceClient>();

            return _simpleEmailClient;
        }
    }
    /// <summary>
    /// Sends a notification via email asynchronously.
    /// </summary>
    /// <param name="message">The notification message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A dynamic response from the email sending operation.</returns>
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
                await SimpleEmailClient.SendEmailAsync(mailRequest, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);

        return response;
    }
    /// <summary>
    /// Disposes of the Amazon Simple Email Service (SES) client instance.
    /// </summary>
    protected override void DisposeServices()
    {
        _simpleEmailClient?.Dispose();
    }
}