// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

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