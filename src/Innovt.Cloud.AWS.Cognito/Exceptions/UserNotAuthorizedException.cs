using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

/// <summary>
///     Exception thrown when a user is not authorized to perform a specific action or access a resource.
/// </summary>
public class UserNotAuthorizedException : BusinessException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UserNotAuthorizedException" /> class with a predefined error code.
    /// </summary>
    public UserNotAuthorizedException() : base(ErrorCode.NotAuthorized, ErrorCode.NotAuthorized)
    {
    }
}