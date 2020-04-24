using System.Collections.Generic;

namespace Innovt.Notification.Core.Domain
{
    public class NotificationRequest
    {
        public string TemplateId { get; set; }
        
        public List<NotificationMessageContact> To { get; set; }

        public object PayLoad { get; set; }

        public NotificationRequest()
        {
            To = new List<NotificationMessageContact>();
        }
    }
}
