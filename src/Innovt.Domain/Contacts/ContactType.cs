// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Innovt.Core.Utilities;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Contacts
{
    /// <summary>
    ///     Email, Phone
    /// </summary>
    public class ContactType : ValueObject<int>
    {
        private ContactType()
        {
        }

        [Required] public string Name { get; set; }

        [Required] public string Description { get; set; }
        public string IconUrl { get; set; }
        public string RegexValidation { get; set; }


        public static ContactType Create(string name, string description)
        {
            return new ContactType
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