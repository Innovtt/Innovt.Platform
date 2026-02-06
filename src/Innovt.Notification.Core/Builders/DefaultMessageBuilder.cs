// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

using System;
using System.Collections.Generic;
using Innovt.Core.Utilities;
using Innovt.Notification.Core.Domain;
using Innovt.Notification.Core.Template;

namespace Innovt.Notification.Core.Builders;

/// <summary>
///     Default implementation of the message builder interface.
/// </summary>
public class DefaultMessageBuilder : IMessageBuilder
{
    private readonly ITemplateParser parser;

    /// <summary>
    ///     Default constructor using a template parser(optional)
    /// </summary>
    /// <param name="parser"></param>
    public DefaultMessageBuilder(ITemplateParser parser = null)
    {
        this.parser = parser;
    }

    /// <summary>
    ///     This method will build the message and parse the result of each content
    /// </summary>
    /// <param name="notificationTemplate"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public NotificationMessage Build(NotificationTemplate notificationTemplate, NotificationRequest request)
    {
        ArgumentNullException.ThrowIfNull(notificationTemplate);
        ArgumentNullException.ThrowIfNull(request);

        var message = new NotificationMessage(notificationTemplate.Type)
        {
            Subject = BuildSubject(notificationTemplate, request),
            From = BuildFrom(notificationTemplate, request),
            To = BuildTo(notificationTemplate, request),
            Body = BuildBody(notificationTemplate, request),
            BccTo = BuildBccTo(notificationTemplate, request),
            CcTo = BuildCcTo(notificationTemplate, request),
            ReplyToAddresses = BuildReplyTo(notificationTemplate, request)
        };

        //TODO: can be better if we parse per type maybe
        ParseMessage(message, request.PayLoad);

        return message;
    }

    /// <summary>
    ///     Builds the subject for the notification message.
    /// </summary>
    protected virtual NotificationMessageSubject BuildSubject(NotificationTemplate template,
        NotificationRequest request)
    {
        ArgumentNullException.ThrowIfNull(template);

        return new NotificationMessageSubject
        {
            Charset = template.Charset,
            Content = template.Subject
        };
    }

    /// <summary>
    ///     Builds the body for the notification message.
    /// </summary>
    protected virtual NotificationMessageBody BuildBody(NotificationTemplate notificationTemplate,
        NotificationRequest request)
    {
        if (notificationTemplate == null) return null;

        return new NotificationMessageBody
        {
            Content = notificationTemplate.Body,
            Charset = notificationTemplate.Charset,
            IsHtml = notificationTemplate.Type == NotificationMessageType.Email
        };
    }

    /// <summary>
    ///     Builds the recipient list for the notification message.
    /// </summary>
    protected virtual List<NotificationMessageContact> BuildTo(NotificationTemplate notificationTemplate,
        NotificationRequest request)
    {
        Check.NotNullWithBusinessException(request?.To,
            $"Invalid ToAddress for template Id {notificationTemplate?.Id}");

        var toList = new List<NotificationMessageContact>();

        foreach (var to in request.To) toList.Add(new NotificationMessageContact(to.Name, to.Address));

        return toList;
    }

    /// <summary>
    ///     Builds the sender for the notification message.
    /// </summary>
    protected virtual NotificationMessageContact BuildFrom(NotificationTemplate notificationTemplate,
        NotificationRequest request)
    {
        Check.NotNullWithBusinessException(notificationTemplate?.FromAddress,
            $"Invalid FromAddress for template Id {notificationTemplate.Id}");

        return new NotificationMessageContact(notificationTemplate.FromName, notificationTemplate.FromAddress);
    }

    /// <summary>
    ///     Builds the Bcc recipient list for the notification message.
    /// </summary>
    protected virtual List<NotificationMessageContact> BuildBccTo(NotificationTemplate template,
        NotificationRequest request)
    {
        return null;
    }

    /// <summary>
    ///     Builds the Cc recipient list for the notification message.
    /// </summary>
    protected virtual List<NotificationMessageContact> BuildCcTo(NotificationTemplate template,
        NotificationRequest request)
    {
        return null;
    }

    /// <summary>
    ///     Builds the Reply-To recipient list for the notification message.
    /// </summary>
    protected virtual List<NotificationMessageContact> BuildReplyTo(NotificationTemplate template,
        NotificationRequest request)
    {
        return null;
    }

    /// <summary>
    ///     Parses the notification message content using the provided payload.
    /// </summary>
    protected virtual void ParseMessage(NotificationMessage notificationMessage, object payLoad)
    {
        if (parser == null || notificationMessage == null)
            return;

        if (notificationMessage.Subject?.Content != null)
            notificationMessage.Subject.Content = parser.Render(notificationMessage.Subject.Content, payLoad);

        if (notificationMessage.Body?.Content != null)
            notificationMessage.Body.Content = parser.Render(notificationMessage.Body.Content, payLoad);

        if (notificationMessage.To != null)
            foreach (var to in notificationMessage.To)
            {
                to.Address = parser.Render(to.Address, payLoad);

                if (!string.IsNullOrEmpty(to.Name))
                    to.Name = parser.Render(to.Name, payLoad);
            }

        if (notificationMessage.BccTo != null)
            foreach (var bccTo in notificationMessage.BccTo)
            {
                bccTo.Address = parser.Render(bccTo.Address, payLoad);

                if (!string.IsNullOrEmpty(bccTo.Name))
                    bccTo.Name = parser.Render(bccTo.Name, payLoad);
            }

        if (notificationMessage.CcTo != null)
            foreach (var ccTo in notificationMessage.CcTo)
            {
                ccTo.Address = parser.Render(ccTo.Address, payLoad);

                if (!string.IsNullOrEmpty(ccTo.Name))
                    ccTo.Name = parser.Render(ccTo.Name, payLoad);
            }

        if (notificationMessage.Subject?.Content != null)
            notificationMessage.Subject.Content = parser.Render(notificationMessage.Subject.Content, payLoad);
    }
}