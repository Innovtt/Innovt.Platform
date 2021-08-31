// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Notification
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Notification.Core;
using Innovt.Notification.Core.Domain;

namespace Innovt.Cloud.AWS.Notification
{
    public class NotificationHandleFactory : INotificationHandleFactory
    {
        private readonly IContainer container;

        public NotificationHandleFactory(IContainer container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public virtual INotificationHandler Create(NotificationMessageType notificationMessageType)
        {
            return notificationMessageType switch
            {
                NotificationMessageType.Email => container.Resolve<MailNotificationHandler>(),
                NotificationMessageType.Sms => container.Resolve<SmsNotificationHandler>(),
                _ => CreateCustomHandle(notificationMessageType)                    
            };
        }

        public virtual INotificationHandler CreateCustomHandle(NotificationMessageType notificationMessageType) {

            return null;
        }
    }
}