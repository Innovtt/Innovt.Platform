using System;
using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

public class UserNotConfirmedException: BusinessException
{
    public UserNotConfirmedException():base(ErrorCode.UserNotConfirmed,ErrorCode.UserNotConfirmed)
    {
        
    }
    
    public UserNotConfirmedException(Exception ex):base(ErrorCode.UserNotConfirmed,ErrorCode.UserNotConfirmed,ex)
    {
        
    }
}