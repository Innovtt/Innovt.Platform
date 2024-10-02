using System;
using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

/// <summary>
///     Exception thrown when a user is not confirmed or validated in the application.
/// </summary>
public class UserNotConfirmedException : BusinessException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UserNotConfirmedException" /> class with a predefined error code.
    /// </summary>
    public UserNotConfirmedException() : base(ErrorCode.UserNotConfirmed, ErrorCode.UserNotConfirmed)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserNotConfirmedException" /> class with a predefined error code and
    ///     an inner exception.
    /// </summary>
    /// <param name="ex">The inner exception that caused this exception to be thrown.</param>
    public UserNotConfirmedException(Exception ex) : base(ErrorCode.UserNotConfirmed, ErrorCode.UserNotConfirmed, ex)
    {
    }
}