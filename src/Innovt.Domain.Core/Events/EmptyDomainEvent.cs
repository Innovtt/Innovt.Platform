// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using Innovt.Domain.Core.Streams;

namespace Innovt.Domain.Core.Events;

/// <summary>
///     Represents an empty domain event used for specific partition in the system.
/// </summary>
internal class EmptyDomainEvent : DomainEvent, IEmptyDataStream
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EmptyDomainEvent" /> class with a specified partition.
    /// </summary>
    /// <param name="partition">The partition associated with the event.</param>
    public EmptyDomainEvent(string? partition) : base("empty", partition)
    {
    }
}