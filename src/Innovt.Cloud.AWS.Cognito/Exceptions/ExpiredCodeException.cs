using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

public class ExpiredCodeException: BusinessException
{
    public ExpiredCodeException():base(ErrorCode.ExpiredCode,ErrorCode.ExpiredCode)
    {
        
    }
}