using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Dynamo.Converters.Attributes.Exceptions;

public class DiscriminatorException : CriticalException
{
    public DiscriminatorException(string message) : base(message)
    {
    }
}