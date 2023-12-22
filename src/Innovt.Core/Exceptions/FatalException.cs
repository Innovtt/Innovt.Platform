// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Runtime.Serialization;

namespace Innovt.Core.Exceptions;

[Serializable]
/// <summary>
/// Represents a fatal exception that may occur during application execution.
/// </summary>
public class FatalException : BaseException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FatalException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the reason for the exception.</param>
    public FatalException(string message) : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FatalException" /> class with a specified error message
    ///     and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that describes the reason for the exception.</param>
    /// <param name="ex">The inner exception that is the cause of this exception.</param>
    public FatalException(string message, Exception ex) : base(message, ex)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FatalException" /> class with a specified inner exception.
    /// </summary>
    /// <param name="ex">The inner exception that is the cause of this exception.</param>
    public FatalException(Exception ex) : base(ex)
    {
    }

    // Private parameterless constructor to support serialization.
    private FatalException()
    {
    }

    // Protected constructor for deserialization support.
    protected FatalException(SerializationInfo serializationInfo,
        StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }
}