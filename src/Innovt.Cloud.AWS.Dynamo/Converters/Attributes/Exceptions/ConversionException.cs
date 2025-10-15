using System;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Dynamo.Converters.Attributes.Exceptions;

public class ConversionException : CriticalException
{
    public ConversionException(string message) : base(message)
    {
    }

    public ConversionException(string message, Exception ex) : base(message, ex)
    {
    }
}