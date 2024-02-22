// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

using System;
using Innovt.Notification.Core.Domain;
using Innovt.Notification.Core.Template;

namespace Innovt.Notification.Core.Builders;

/// <summary>
///     Abstract base class for message builders.
/// </summary>
public abstract class MessageBuilderAB
{
    private readonly ITemplateParser parser;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MessageBuilderAB" /> class.
    /// </summary>
    /// <param name="parser">The template parser.</param>
    protected MessageBuilderAB(ITemplateParser parser)
    {
        this.parser = parser ?? throw new ArgumentNullException(nameof(parser));
    }

    /// <summary>
    ///     Builds a notification message based on the provided notification template and request.
    /// </summary>
    /// <param name="template">The notification template.</param>
    /// <param name="request">The notification request.</param>
    /// <returns>The built notification message.</returns>
    public virtual NotificationMessage Build(NotificationTemplate template, NotificationRequest request)
    {
        if (template == null) throw new ArgumentNullException(nameof(template));
        if (request == null) throw new ArgumentNullException(nameof(request));

        var message =
            new NotificationMessage(template.Type, template.FromAddress, template.FromName, template.Subject)
            {
                Body = new NotificationMessageBody
                {
                    Content = template.Body,
                    Charset = template.Charset
                    //IsHtml = template.Type == Innovt.Core.Notification.NotificationMessageType.Email
                }
            };

        foreach (var to in request.To) message.AddTo(to.Name, to.Address);

        ParseMessage(message, request.PayLoad);

        return message;
    }

    /// <summary>
    ///     Parses the notification message content using the provided payload.
    /// </summary>
    protected virtual void ParseMessage(NotificationMessage message, object payLoad)
    {
        if (message.Body == null && message.To == null && message.Subject == null)
            return;

        if (message.Body != null) message.Body.Content = parser.Render(message.Body.Content, payLoad);

        if (message.To != null)
            foreach (var to in message.To)
            {
                to.Address = parser.Render(to.Address, payLoad);

                if (!string.IsNullOrEmpty(to.Name))
                    to.Name = parser.Render(to.Name, payLoad);
            }

        if (message.BccTo != null)
            foreach (var bccTo in message.BccTo)
            {
                bccTo.Address = parser.Render(bccTo.Address, payLoad);

                if (!string.IsNullOrEmpty(bccTo.Name))
                    bccTo.Name = parser.Render(bccTo.Name, payLoad);
            }

        if (message.CcTo != null)
            foreach (var ccTo in message.CcTo)
            {
                ccTo.Address = parser.Render(ccTo.Address, payLoad);

                if (!string.IsNullOrEmpty(ccTo.Name))
                    ccTo.Name = parser.Render(ccTo.Name, payLoad);
            }

        if (message.Subject != null) message.Subject.Content = parser.Render(message.Subject.Content, payLoad);
    }
}