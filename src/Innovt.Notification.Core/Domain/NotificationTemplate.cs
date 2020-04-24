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
