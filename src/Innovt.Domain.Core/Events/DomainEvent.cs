using Innovt.Domain.Core.Streams;
using System;

namespace Innovt.Domain.Core.Events
{
    public abstract class DomainEvent : IDataStream
    {
        public string Name { get; set; }

        public string EventId { get; set; }
        public string Version { get; set; }
        public string Partition { get; set; }
        public string TraceId { get; set; }

        public DateTime ApproximateArrivalTimestamp { get; set; }
        public DateTime CreatedAt { get; set; }

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
    }
}