using System;
using System.Runtime.Serialization;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Cognito.Exceptions;

public class InternalException:CriticalException
{
    public InternalException(string message) : base(message)
    {
    }

    public InternalException(string message, Exception ex) : base(message, ex)
    {
    }

    public InternalException(Exception ex) : base(ex)
    {
    }

    protected InternalException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }
}