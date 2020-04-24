using System;
using System.Collections.Generic;
using Innovt.Core.Utilities;
using Innovt.Notification.Core.Domain;
using Innovt.Notification.Core.Template;

namespace Innovt.Notification.Core.Builders
{
    public class DefaultMessageBuilder : IMessageBuilder
    {
        private readonly ITemplateParser parser;
       
       /// <summary>
       /// Default constructor using a template parser(optional)
       /// </summary>
       /// <param name="parser"></param>
       public DefaultMessageBuilder(ITemplateParser parser=null)
       {
           this.parser = parser;
       }

       protected virtual NotificationMessageSubject BuildSubject(NotificationTemplate template, NotificationRequest request)
       {
           return new NotificationMessageSubject()
           {
               Charset = template.Charset,
               Content = template.Subject
           };
       }
       
       protected virtual NotificationMessageBody BuildBody(NotificationTemplate template, NotificationRequest request)
       {
           return new NotificationMessageBody()
           {
               Content = template.Body,
               Charset = template.Charset,
               IsHtml = template.Type == NotificationMessageType.Email
           };
       }

       protected virtual List<NotificationMessageContact> BuildTo(NotificationTemplate template, NotificationRequest request)
       {
           Check.NotNullWithBusinessException(request.To,$"Invalid ToAddress for template Id {template.Id}");

           var toList = new List<NotificationMessageContact>();

           foreach (var to in request.To)
           {
               toList.Add(new NotificationMessageContact(to.Name, to.Address));
           }

           return toList;
       }

       protected virtual NotificationMessageContact BuildFrom(NotificationTemplate template, NotificationRequest request)
       {
           Check.NotNullWithBusinessException(template.FromAddress, $"Invalid FromAddress for template Id {template.Id}");

           return new NotificationMessageContact(template.FromName,template.FromAddress);
        }

       protected virtual List<NotificationMessageContact> BuildBccTo(NotificationTemplate template, NotificationRequest request)
       {
           return null;
       }

        protected virtual List<NotificationMessageContact> BuildCcTo(NotificationTemplate template, NotificationRequest request)
        {
            return null;
        }

        protected virtual List<NotificationMessageContact> BuildReplyTo(NotificationTemplate template, NotificationRequest request)
        {
            return null;
        }

        /// <summary>
        /// This method will build the message and parse the result of each content
        /// </summary>
        /// <param name="template"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public NotificationMessage Build(NotificationTemplate template, NotificationRequest request)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (request == null) throw new ArgumentNullException(nameof(request));

            var message = new NotificationMessage(template.Type)
            {
                Subject = BuildSubject(template, request),
                From    = BuildFrom(template, request),
                To      = BuildTo(template, request),
                Body    = BuildBody(template, request),
                BccTo   = BuildBccTo(template, request),
                CcTo    = BuildCcTo(template, request),
                ReplyToAddresses    = BuildReplyTo(template, request)
            };
           
            //TODO: can be better if we parse per type maybe
           ParseMessage(message,request.PayLoad);

            return message;
        }

        protected virtual void ParseMessage(NotificationMessage message,object payLoad)
        {
            if (parser == null)
                return;
            
            if (message.Subject != null)
            {
                message.Subject.Content = parser.Render(message.Subject.Content, payLoad);
            }

            if (message.Body != null)
            {
                message.Body.Content = parser.Render(message.Body.Content, payLoad);
            }

            if (message.To != null)
            {
                foreach (var to in message.To)
                {
                    to.Address = parser.Render(to.Address, payLoad);

                    if (!string.IsNullOrEmpty(to.Name))
                        to.Name = parser.Render(to.Name, payLoad);
                }
            }

            if (message.BccTo != null)
            {
                foreach (var bccTo in message.BccTo)
                {
                    bccTo.Address = parser.Render(bccTo.Address, payLoad);

                    if (!string.IsNullOrEmpty(bccTo.Name))
                        bccTo.Name = parser.Render(bccTo.Name, payLoad);
                }
            }

            if (message.CcTo != null)
            {
                foreach (var ccTo in message.CcTo)
                {
                    ccTo.Address = parser.Render(ccTo.Address, payLoad);

                    if (!string.IsNullOrEmpty(ccTo.Name))
                        ccTo.Name = parser.Render(ccTo.Name, payLoad);
                }
            }

            if (message.Subject != null)
            {
                message.Subject.Content = parser.Render(message.Subject.Content, payLoad);
            }
        }
    }
}