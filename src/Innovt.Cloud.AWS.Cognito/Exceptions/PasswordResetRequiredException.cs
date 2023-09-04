using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

public class PasswordResetRequiredException: BusinessException
{
    public PasswordResetRequiredException():base(ErrorCode.PasswordResetRequired,ErrorCode.PasswordResetRequired)
    {
        
    }
}