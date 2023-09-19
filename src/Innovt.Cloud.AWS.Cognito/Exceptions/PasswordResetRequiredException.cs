using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

/// <summary>
/// Exception thrown when a password reset is required for a user or system operation.
/// This exception typically indicates that a user's password has expired or that a password reset has been initiated.
/// </summary>
public class PasswordResetRequiredException: BusinessException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordResetRequiredException"/> class with a predefined error code.
    /// </summary>
    public PasswordResetRequiredException():base(ErrorCode.PasswordResetRequired,ErrorCode.PasswordResetRequired)
    {
        
    }
}