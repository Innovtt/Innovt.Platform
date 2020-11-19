using System;

namespace Innovt.Domain.Core.Streams
{
    public interface IDataStream
    {
        public string EventId { get; set; }
        public string Partition { get; set; }
        public string TraceId { get; set; }
        public DateTime ApproximateArrivalTimestamp { get; set; }
    }
}