﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public class ChangePasswordRequest: RequestBase
    { 
        [Required]
        public string AccessToken { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string PreviousPassword { get; set; }

        [Required]
        [StringLength(18, MinimumLength = 4)]
        public string ProposedPassword { get; set; }

        [Required]
        [StringLength(18, MinimumLength = 4)]
        public string ConfirmProposedPassword { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AccessToken.IsNullOrEmpty())
            {
                yield return new ValidationResult(Messages.EmailIsRequired, new[] { nameof(AccessToken) });
            }
            
            if (ConfirmProposedPassword.IsNullOrEmpty())
            {
                yield return new ValidationResult(Messages.ConfirmPasswordIsRequired, new[] { nameof(ConfirmProposedPassword) });
            }  
            
            if (PreviousPassword.IsNullOrEmpty())
            {
                yield return new ValidationResult(Messages.CurrentPasswordRequired, new[] { nameof(PreviousPassword) });
            }
            
            if (ProposedPassword != ConfirmProposedPassword)
            {
                yield return new ValidationResult(Messages.PasswordsDoNotMatch, new[] { nameof(ConfirmProposedPassword), nameof(ProposedPassword) });
            }
        }
    }
}
