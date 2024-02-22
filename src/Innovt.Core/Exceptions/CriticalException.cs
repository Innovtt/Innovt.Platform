// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Runtime.Serialization;

namespace Innovt.Core.Exceptions;

[Serializable]
/// <summary>
/// Represents an exception that is critical and requires immediate attention or action.
/// </summary>
public class CriticalException : BaseException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CriticalException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the critical issue.</param>
    public CriticalException(string message) : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CriticalException" /> class with a specified error message and inner
    ///     exception.
    /// </summary>
    /// <param name="message">The error message that describes the critical issue.</param>
    /// <param name="ex">The inner exception that caused the critical issue.</param>
    public CriticalException(string message, Exception ex) : base(message, ex)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CriticalException" /> class with a specified inner exception.
    /// </summary>
    /// <param name="ex">The inner exception that caused the critical issue.</param>
    public CriticalException(Exception ex) : base(ex)
    {
    }

    // Private constructor to prevent instantiation without parameters.
    private CriticalException()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CriticalException" /> class with serialized data.
    /// </summary>
    /// <param name="serializationInfo">The <see cref="SerializationInfo" /> containing serialized object data.</param>
    /// <param name="streamingContext">
    ///     The <see cref="StreamingContext" /> representing the source or destination of the
    ///     serialized data.
    /// </param>
    protected CriticalException(SerializationInfo serializationInfo,
        StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }
}