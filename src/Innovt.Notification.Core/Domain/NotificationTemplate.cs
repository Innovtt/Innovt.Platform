// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Notification.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

namespace Innovt.Notification.Core.Domain
{
    public class NotificationTemplate
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string TemplateUrl { get; set; }
        public string Charset { get; set; }
        public string Body { get; set; }
        public string Builder { get; set; }
        public NotificationMessageType Type { get; set; }
    }
}