using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

/// <summary>
/// Exception thrown when attempting to create a user account with a username that already exists in the system.
/// </summary>
public class UserNameAlreadyExistsException : BusinessException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserNameAlreadyExistsException"/> class with a predefined error code.
    /// </summary>
    public UserNameAlreadyExistsException():base(ErrorCode.UsernameAlreadyExists,ErrorCode.UsernameAlreadyExists)
    {
        
    }
    
}