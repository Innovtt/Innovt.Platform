// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Domain.Core.Events;

public interface IEventHandler
{
    Task Publish(DomainEvent domainEvent, CancellationToken cancellationToken = default);
    Task Publish(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken = default);
}