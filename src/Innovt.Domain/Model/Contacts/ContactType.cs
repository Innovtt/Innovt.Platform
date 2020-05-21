using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Innovt.Core.Utilities;

namespace Innovt.Domain.Model.Contacts
{
    public class ContactType:ValueObject<int>
    {
        private ContactType() {}

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public string RegexValidation { get; set; }


        public static ContactType Create(string name,string description)
        {
            return new ContactType()
            {
                Name = name,
                Description = description
            };
        }

        public virtual bool Validate(string value)
        {
            if (!RegexValidation.IsNull())
                return true;

            var regEx = new Regex(RegexValidation);

            return regEx.IsMatch(value);
        }
    }
}