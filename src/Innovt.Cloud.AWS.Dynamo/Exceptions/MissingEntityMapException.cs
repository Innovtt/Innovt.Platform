using System;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Dynamo.Exceptions;

public class MissingEntityMapException:CriticalException
{
    public MissingEntityMapException(Type type) : base($"The map of type {type.FullName} of entity was not found in the model")  
    {
    }
    
    public MissingEntityMapException(string typeName) : base($"The map of type {typeName} of entity was not found in the model")  
    {
    }
}