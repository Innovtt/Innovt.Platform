using System;
using System.Runtime.Serialization;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

/// <summary>
/// Exception thrown when an internal critical error occurs.
/// This type of exception is used for errors that are considered critical and may require special handling.
/// </summary>
public class InternalException : CriticalException
{
    // <summary>
    /// Initializes a new instance of the <see cref="InternalException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    public InternalException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InternalException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    /// <param name="ex">The inner exception that is the cause of this exception.</param>
    public InternalException(string message, Exception ex) : base(message, ex)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InternalException"/> class with a reference to the inner exception
    /// that is the cause of this exception.
    /// </summary>
    /// <param name="ex">The inner exception that is the cause of this exception.</param>
    public InternalException(Exception ex) : base(ex)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InternalException"/> class with serialized data.
    /// </summary>
    /// <param name="serializationInfo">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="streamingContext">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
    protected InternalException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(
        serializationInfo, streamingContext)
    {
    }
}