// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

namespace Innovt.Core.Exceptions;

/// <summary>
///     You can use it to create custom error messages that will be used by our framework.
/// </summary>
public class ErrorMessage
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="message">The message that you want to send.</param>
    public ErrorMessage(string message)
    {
        Message = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorMessage"/> class with default values.
    /// </summary>
    public ErrorMessage()
    {
    }

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="message">The message that you want to send.</param>
    /// <param name="propertyName">The property(optional) that this error happened.</param>
    public ErrorMessage(string message, string propertyName)
    {
        Message = message;
        PropertyName = propertyName;
    }

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="message">The message that you want to send.</param>
    /// <param name="propertyName">The property(optional) that this error happened.</param>
    /// <param name="code">The code of you error</param>
    public ErrorMessage(string message, string propertyName, string code)
    {
        Message = message;
        PropertyName = propertyName;
        Code = code;
    }

    /// <summary>
    /// Gets or sets the error code associated with the error message.
    /// </summary>
    public string Code { get; protected set; }
    /// <summary>
    /// Gets or sets the error message describing the error.
    /// </summary>
    public string Message { get; protected set; }
    /// <summary>
    /// Gets or sets the name of the property to which the error is related.
    /// </summary>
    public string PropertyName { get; protected set; }
}