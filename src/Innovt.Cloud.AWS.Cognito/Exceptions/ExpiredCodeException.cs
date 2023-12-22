using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

/// <summary>
///     Exception thrown when an expired code error occurs.
///     This typically happens when a time-limited code or token has expired and is no longer valid.
/// </summary>
public class ExpiredCodeException : BusinessException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ExpiredCodeException" /> class.
    /// </summary>
    public ExpiredCodeException() : base(ErrorCode.ExpiredCode, ErrorCode.ExpiredCode)
    {
    }
}