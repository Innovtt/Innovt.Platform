using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public class SignInRequest : SignInRequestBase
    {
        internal const string ValidateContextCreate = "Create";

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public virtual string Password { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserName.IsNullOrEmpty())
                yield return new ValidationResult(Messages.EmailIsRequired, new[] {nameof(UserName)});

            if (validationContext.ObjectInstance != null &&
                ValidateContextCreate.Equals(validationContext.ObjectInstance.ToString()))
            {
                if (IpAddress.IsNullOrEmpty())
                    yield return new ValidationResult(Messages.IpAddressRequired, new[] {nameof(IpAddress)});

                if (ServerPath.IsNullOrEmpty())
                    yield return new ValidationResult(Messages.ServerPathRequired, new[] {nameof(ServerPath)});
            }
        }
    }
}