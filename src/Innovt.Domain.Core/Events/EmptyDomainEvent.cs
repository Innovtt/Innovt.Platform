// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using Innovt.Domain.Core.Streams;

namespace Innovt.Domain.Core.Events;

internal class EmptyDomainEvent : DomainEvent, IEmptyDataStream
{
    public EmptyDomainEvent(string partition) : base("empty", partition)
    {
    }
}