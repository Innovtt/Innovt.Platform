using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

/// <summary>
///     Exception thrown when a user is not found in the application.
/// </summary>
public class UserNotFoundException : BusinessException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UserNotFoundException" /> class with a predefined error code.
    /// </summary>
    public UserNotFoundException() : base(ErrorCode.UserNotFound, ErrorCode.UserNotFound)
    {
    }
}