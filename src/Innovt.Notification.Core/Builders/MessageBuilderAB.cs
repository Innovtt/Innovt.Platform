using Innovt.Notification.Core.Domain;
using Innovt.Notification.Core.Template;
using System;

namespace Innovt.Notification.Core.Builders
{
    public abstract class MessageBuilderAB
    {
        private readonly ITemplateParser parser;

        protected MessageBuilderAB(ITemplateParser parser)
        {
            this.parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public virtual NotificationMessage Build(NotificationTemplate template, NotificationRequest request)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (request == null) throw new ArgumentNullException(nameof(request));

            var message =
                new NotificationMessage(template.Type, template.FromAddress, template.FromName, template.Subject)
                {
                    Body = new NotificationMessageBody()
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
}