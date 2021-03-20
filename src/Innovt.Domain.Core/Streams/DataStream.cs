using System;

namespace Innovt.Domain.Core.Streams
{
    public class DataStream<T> : IDataStream<T> where T : class
    {
        public string Version { get; set; }
        public string EventId { get; set; }
        public string Partition { get; set; }
        public string TraceId { get; set; }
        public DateTime ApproximateArrivalTimestamp { get; set; }

        public T Body { get; set; }

        public DataStream()
        {
        }

        public DataStream(string version, string partition, string traceId, T body)
        {
            Version = version;
            Partition = partition;
            TraceId = traceId;
            Body = body;
        }
    }
}