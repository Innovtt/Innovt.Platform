// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Runtime.Serialization;

namespace Innovt.Core.Exceptions;

[Serializable]
/// <summary>
/// Represents an exception that occurs when there is an issue with configuration settings.
/// </summary>
public class ConfigurationException : BaseException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ConfigurationException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the issue with the configuration.</param>
    public ConfigurationException(string message) : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConfigurationException" /> class with a specified error message and
    ///     inner exception.
    /// </summary>
    /// <param name="message">The error message that describes the issue with the configuration.</param>
    /// <param name="ex">The inner exception that caused the configuration issue.</param>
    public ConfigurationException(string message, Exception ex) : base(message, ex)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConfigurationException" /> class with a specified inner exception.
    /// </summary>
    /// <param name="ex">The inner exception that caused the configuration issue.</param>
    public ConfigurationException(Exception ex) : base(ex)
    {
    }

    // Private constructor to prevent instantiation without parameters
    private ConfigurationException()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConfigurationException" /> class with serialized data.
    /// </summary>
    /// <param name="serializationInfo">The <see cref="SerializationInfo" /> containing serialized object data.</param>
    /// <param name="streamingContext">
    ///     The <see cref="StreamingContext" /> representing the source or destination of the
    ///     serialized data.
    /// </param>
    protected ConfigurationException(SerializationInfo serializationInfo,
        StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }
}