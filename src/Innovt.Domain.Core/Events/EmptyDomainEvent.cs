// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Core.Streams;

namespace Innovt.Domain.Core.Events
{
    internal class EmptyDomainEvent : DomainEvent, IEmptyDataStream
        //, IEmptyDomainEvent
    {
        public EmptyDomainEvent(string partition) : base("empty", partition)
        {
            
        }
    }
}