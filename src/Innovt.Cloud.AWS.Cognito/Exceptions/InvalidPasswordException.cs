using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

/// <summary>
///     Exception thrown when an invalid password is encountered.
///     This exception typically indicates that a password provided by a user or a system does not meet the required
///     criteria or is incorrect.
/// </summary>
public class InvalidPasswordException : BusinessException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InvalidPasswordException" /> class with a predefined error code.
    /// </summary>
    public InvalidPasswordException() : base(ErrorCode.InvalidPassword, ErrorCode.InvalidPassword)
    {
    }
}