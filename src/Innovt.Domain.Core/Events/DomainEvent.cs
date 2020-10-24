using System;

namespace Innovt.Domain.Core.Events
{
    public abstract class DomainEvent
    {
        public string Name { get; }
        public string Version { get; }
        public string TraceId { get; set; }
        public DateTimeOffset CreatedAt { get; }

        protected DomainEvent(string name, string version, string traceId)
        {
            Name = name;
            Version = version;
            TraceId = traceId;
            CreatedAt =  DateTimeOffset.UtcNow;
            Version = version ?? "1.0";
        }
    }
}