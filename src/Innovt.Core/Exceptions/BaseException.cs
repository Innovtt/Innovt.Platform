// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Runtime.Serialization;

namespace Innovt.Core.Exceptions;

[Serializable]
public class BaseException : Exception
{
    public BaseException()
    {
    }

    public BaseException(string message)
        : this(message, null)
    {
    }

    public BaseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public BaseException(Exception innerException)
        : base(null, innerException)
    {
    }

    protected BaseException(SerializationInfo serializationInfo,
        StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }
}