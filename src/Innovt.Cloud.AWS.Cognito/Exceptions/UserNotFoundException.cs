using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions
{
    public class UserNotFoundException: BusinessException
    {
        public UserNotFoundException():base(ErrorCode.UserNotFound)
        {

        }
    }
}
