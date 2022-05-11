// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

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