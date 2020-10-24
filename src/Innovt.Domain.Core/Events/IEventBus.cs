using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Domain.Core.Events
{
    public interface IEventBus
    {
        Task Publish(DomainEvent @event, CancellationToken cancellationToken = default);
        Task Publish(List<DomainEvent> events, CancellationToken cancellationToken = default);
    }
}
