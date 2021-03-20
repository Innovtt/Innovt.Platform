using Innovt.Core.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Notification.Core.Domain
{
    public class NotificationRequest : IValidatableObject
    {
        public string TemplateId { get; set; }

        public List<NotificationMessageContact> To { get; set; }

        public object PayLoad { get; set; }

        public NotificationRequest()
        {
            To = new List<NotificationMessageContact>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TemplateId.IsNullOrEmpty())
            {
                yield return new ValidationResult("TemplateId can't be null or empty.");
            }


            if (To.IsNullOrEmpty())
            {
                yield return new ValidationResult("The To can't be empty.");
            }
            else
            {
                foreach (var to in To)
                {
                    var items = to.Validate(validationContext);

                    foreach (var item in items)
                        yield return item;
                }
            }
        }
    }
}