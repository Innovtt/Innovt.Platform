using System;
using System.Diagnostics.CodeAnalysis;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Dynamo.Exceptions;

/// <summary>
/// THis exception is thrown when there is no discriminator for the provided value or type.
/// </summary>
[SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable")]
public class InvalidDiscriminatorException : CriticalException
{
    public InvalidDiscriminatorException(object value) : base(
        $"There is no discriminator for the provided value {value}.Please check your context mapping.")
    {
    }

    public InvalidDiscriminatorException(Type type) : base(
        $"There is no discriminator for the provided type {type}.Please check your context mapping.")
    {
    } 
}