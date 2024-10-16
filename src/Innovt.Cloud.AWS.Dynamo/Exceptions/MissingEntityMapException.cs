using System;
using System.Diagnostics.CodeAnalysis;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Dynamo.Exceptions;

[SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable")]
public class MissingEntityMapException : CriticalException
{
    public MissingEntityMapException(Type type) : base(
        $"The map of type {type.FullName} of entity was not found in the model")
    {
    }

    public MissingEntityMapException(string typeName) : base(
        $"The map of type {typeName} of entity was not found in the model")
    {
    }
}