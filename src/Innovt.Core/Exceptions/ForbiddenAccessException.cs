namespace Innovt.Core.Exceptions;

/// <summary>
/// The exception that is thrown when a user is valid, but the server is refusing action.
/// </summary>
public class ForbiddenAccessException : BaseException
{
    public ForbiddenAccessException(string message) : base(message)
    {
    }

    public ForbiddenAccessException()
    {
    }
}