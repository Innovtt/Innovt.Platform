// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.EventBridge

using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.EventBridge;
#pragma warning disable CA1032 // Implement standard exception constructors
/// <summary>
///     Exception thrown when the event limit for an EventBridge request is invalid. EventBridge supports up to 10 entries per
///     PutEvents request.
/// </summary>
internal class InvalidEventLimitException : BaseException
#pragma warning restore CA1032 // Implement standard exception constructors
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InvalidEventLimitException" /> class.
    /// </summary>
    internal InvalidEventLimitException() : base(
        "Invalid event limit. EventBridge supports up to 10 entries per PutEvents request.")
    {
    }
}