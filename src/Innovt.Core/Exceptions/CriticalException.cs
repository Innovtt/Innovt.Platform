// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Runtime.Serialization;

namespace Innovt.Core.Exceptions;

[Serializable]
public class CriticalException : BaseException
{
    public CriticalException(string message) : base(message)
    {
    }

    public CriticalException(string message, Exception ex) : base(message, ex)
    {
    }

    public CriticalException(Exception ex) : base(ex)
    {
    }

    private CriticalException()
    {
    }

    protected CriticalException(SerializationInfo serializationInfo,
        StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }
}