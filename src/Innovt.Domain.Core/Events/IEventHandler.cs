// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Domain.Core.Events;
/// <summary>
/// Defines methods for publishing domain events.
/// </summary>
public interface IEventHandler
{
    /// <summary>
    /// Publishes a single domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to be published.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Publish(DomainEvent domainEvent, CancellationToken cancellationToken = default);
    /// <summary>
    /// Publishes a collection of domain events.
    /// </summary>
    /// <param name="domainEvents">The collection of domain events to be published.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Publish(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken = default);
}