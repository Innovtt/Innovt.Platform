// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Kinesis

using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Kinesis;
#pragma warning disable CA1032 // Implement standard exception constructors
/// <summary>
/// Exception thrown when the event limit for a Kinesis request is invalid. Kinesis supports up to 500 messages per request.
/// </summary>
internal class InvalidEventLimitException : BaseException
#pragma warning restore CA1032 // Implement standard exception constructors
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidEventLimitException"/> class.
    /// </summary>
    internal InvalidEventLimitException() : base(
        "Invalid event limit. Kinesis supports up to 500 messages per request.")
    {
    }
}