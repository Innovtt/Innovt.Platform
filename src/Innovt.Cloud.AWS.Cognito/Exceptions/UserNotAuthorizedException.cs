using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

public class UserNotAuthorizedException: BusinessException
{
    public UserNotAuthorizedException():base(ErrorCode.NotAuthorized,ErrorCode.NotAuthorized)
    {
        
    }
}