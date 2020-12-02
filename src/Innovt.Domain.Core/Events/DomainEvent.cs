using System;
using Innovt.Domain.Core.Streams;

namespace Innovt.Domain.Core.Events
{
    public abstract class DomainEvent: IDataStream
    {
        public string Name { get; }
        public string Version { get; }

        public string EventId { get; set; }
        public string Partition { get; set; }
        public string TraceId { get; set; }
        
        public DateTime ApproximateArrivalTimestamp { get; set; }


        public DateTime CreatedAt { get; set; }
        
        public DomainEvent(string name, string version,string partition)
        {
            Name = name;
            Version = version;
            Partition = partition;
            CreatedAt =  DateTime.UtcNow;
        }
    }
}