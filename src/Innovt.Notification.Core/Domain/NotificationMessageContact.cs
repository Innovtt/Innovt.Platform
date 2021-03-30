using Innovt.Core.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Notification.Core.Domain
{
    public class NotificationMessageContact : IValidatableObject
    {
        public NotificationMessageContact(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public NotificationMessageContact()
        {
        }

        public string Name { get; set; }
        public string Address { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Address.IsNullOrEmpty()) yield return new ValidationResult("Invalid address");
        }
    }
}