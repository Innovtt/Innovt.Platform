using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Domain.Core.Events
{
    public interface IEventHandler
    {
        Task Publish(DomainEvent @event, CancellationToken cancellationToken = default);
        Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken = default);
    }
}
