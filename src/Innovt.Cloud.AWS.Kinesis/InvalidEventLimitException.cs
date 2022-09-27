// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Kinesis

using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Kinesis;
#pragma warning disable CA1032 // Implement standard exception constructors
internal class InvalidEventLimitException : BaseException
#pragma warning restore CA1032 // Implement standard exception constructors
{
    internal InvalidEventLimitException() : base(
        "Invalid event limit. Kinesis supports up to 500 messages per request.")
    {
    }
}