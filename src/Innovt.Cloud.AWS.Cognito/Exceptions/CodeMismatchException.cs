using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

/// <summary>
/// Exception thrown when a code mismatch error occurs. 
/// This typically happens when a code provided by the user does not match the expected code.
/// </summary>
public class CodeMismatchException : BusinessException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CodeMismatchException"/> class.
    /// </summary>
    public CodeMismatchException() : base(ErrorCode.CodeMismatch, ErrorCode.CodeMismatch)
    {
    }
}