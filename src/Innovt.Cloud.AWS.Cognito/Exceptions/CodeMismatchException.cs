using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

public class CodeMismatchException: BusinessException
{
    public CodeMismatchException():base(ErrorCode.CodeMismatch,ErrorCode.CodeMismatch)
    {
        
    }
}