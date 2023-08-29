using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

public class UserNameAlreadyExistsException : BusinessException
{
    public UserNameAlreadyExistsException():base(ErrorCode.UsernameAlreadyExists,ErrorCode.UsernameAlreadyExists)
    {
        
    }
    
}