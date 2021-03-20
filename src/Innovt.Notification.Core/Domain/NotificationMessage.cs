using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Innovt.Core.Collections;
using Innovt.Core.Utilities;

namespace Innovt.Notification.Core.Domain
{
    public class NotificationMessage : IValidatableObject
    {
        public NotificationMessageContact From { get; internal set; }

        public List<NotificationMessageContact> To { get; internal set; }

        public List<NotificationMessageContact> BccTo { get; internal set; }

        public List<NotificationMessageContact> CcTo { get; internal set; }

        public List<NotificationMessageContact> ReplyToAddresses { get; internal set; }

        public NotificationMessageType Type { get; set; }

        public NotificationMessageSubject Subject { get; set; }
        public NotificationMessageBody Body { get; set; }

        public NotificationMessage(NotificationMessageType type)
        {
            Type = type;
        }

        public NotificationMessage(NotificationMessageType type, string fromAddress, string fromName, string subject)
        {
            Type = type;

            if (fromAddress.IsNotNullOrEmpty()) AddFrom(fromAddress, fromName);

            if (subject.IsNotNullOrEmpty()) AddSubject(subject);
        }

        public NotificationMessage AddSubject(string subject, string charset = null)
        {
            Subject = new NotificationMessageSubject()
            {
                Charset = charset.GetValueOrDefault("UTF-8"),
                Content = subject
            };

            return this;
        }

        public NotificationMessage AddFrom(string address, string name = null)
        {
            From = new NotificationMessageContact(name, address);

            return this;
        }

        public NotificationMessage AddTo(string address, string name = null)
        {
            To = To.AddFluent(new NotificationMessageContact(name, address));

            return this;
        }

        public virtual NotificationMessage AddBccTo(string address, string name = null)
        {
            BccTo = BccTo.AddFluent(new NotificationMessageContact(name, address));

            return this;
        }

        public virtual NotificationMessage AddCcTo(string address, string name = null)
        {
            CcTo = CcTo.AddFluent(new NotificationMessageContact(name, address));

            return this;
        }

        public virtual NotificationMessage AddReplyTo(string address, string name = null)
        {
            ReplyToAddresses = ReplyToAddresses.AddFluent(new NotificationMessageContact(name, address));

            return this;
        }

        //public string GetMD5Hash()
        //{
        //    var destinations = string.Join(",", To.Select(t => t.Address));

        //    var hashContent = $"{From?.Address}-{destinations}-{Type}-{Body.Content}";

        //    return hashContent.ComputeMD5();
        //}

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (To == null || !To.Any()) yield return new ValidationResult("Invalid value for To", new[] {"To"});

            if (Body == null || Body.Content.IsNullOrEmpty())
                yield return new ValidationResult("Invalid value for Body", new[] {"Body"});

            if (From == null || From.Address.IsNullOrEmpty())
                yield return new ValidationResult("Invalid value for From", new[] {"From"});

            if (Type == NotificationMessageType.Sms)
                foreach (var to in To)
                    if (to.Address == null ||
                        !to.Address.StartsWith("+", System.StringComparison.InvariantCultureIgnoreCase))
                        yield return new ValidationResult(
                            "Invalid value for To that should start with + and E.164 format.", new[] {"To"});
        }
    }
}