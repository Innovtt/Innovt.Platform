// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Domain.Core.Streams;

namespace Innovt.Domain.Core.Events
{
    public abstract class DomainEvent : IDataStream
    {
        protected DomainEvent(string name, string version, string partition)
        {
            Name = name;
            Version = version;
            Partition = partition;
            CreatedAt = DateTime.UtcNow;
        }

        protected DomainEvent(string name, string partition)
        {
            Name = name;
            Partition = partition;
            CreatedAt = DateTime.UtcNow;
        }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public string EventId { get; set; }
        
        public string Version { get; set; }
        
        public string Partition { get; set; }

        public string TraceId { get; set; }

        public DateTime ApproximateArrivalTimestamp { get; set; }
    }
}