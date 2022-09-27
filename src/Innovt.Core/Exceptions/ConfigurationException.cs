// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Runtime.Serialization;

namespace Innovt.Core.Exceptions;

[Serializable]
public class ConfigurationException : BaseException
{
    public ConfigurationException(string message) : base(message)
    {
    }

    public ConfigurationException(string message, Exception ex) : base(message, ex)
    {
    }

    public ConfigurationException(Exception ex) : base(ex)
    {
    }

    private ConfigurationException()
    {
    }

    protected ConfigurationException(SerializationInfo serializationInfo,
        StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }
}