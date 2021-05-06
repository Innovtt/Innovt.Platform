// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Notification.Core
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Collections;

namespace Innovt.Notification.Core.Domain
{
    public class NotificationRequest : IValidatableObject
    {
        public NotificationRequest()
        {
            To = new List<NotificationMessageContact>();
        }

        public string TemplateId { get; set; }

        public List<NotificationMessageContact> To { get; set; }

        public object PayLoad { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TemplateId.IsNullOrEmpty()) yield return new ValidationResult("TemplateId can't be null or empty.");


            if (To.IsNullOrEmpty())
                yield return new ValidationResult("The To can't be empty.");
            else
                foreach (var to in To)
                {
                    var items = to.Validate(validationContext);

                    foreach (var item in items)
                        yield return item;
                }
        }
    }
}