
namespace Innovt.Notification.Core.Domain
{
    public class NotificationMessageContact
    {
        public NotificationMessageContact(string name, string address)
        {
            Name = name;
            Address = address;
        }
        
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
