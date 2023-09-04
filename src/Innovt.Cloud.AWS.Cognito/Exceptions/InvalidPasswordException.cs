using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

public class InvalidPasswordException: BusinessException
{
    public InvalidPasswordException():base(ErrorCode.InvalidPassword,ErrorCode.InvalidPassword)
    {
        
    }
}