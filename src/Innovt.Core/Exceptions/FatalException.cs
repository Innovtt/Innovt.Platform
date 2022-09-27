// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Runtime.Serialization;

namespace Innovt.Core.Exceptions;

[Serializable]
public class FatalException : BaseException
{
    public FatalException(string message) : base(message)
    {
    }

    public FatalException(string message, Exception ex) : base(message, ex)
    {
    }

    public FatalException(Exception ex) : base(ex)
    {
    }


    private FatalException()
    {
    }

    protected FatalException(SerializationInfo serializationInfo,
        StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }
}