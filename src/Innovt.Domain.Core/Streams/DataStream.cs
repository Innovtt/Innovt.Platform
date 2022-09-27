// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System;

namespace Innovt.Domain.Core.Streams;

public class DataStream<T> : IDataStream<T> where T : class
{
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

    public string Version { get; set; }
    public string EventId { get; set; }
    public string Partition { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public string TraceId { get; set; }
    public DateTime ApproximateArrivalTimestamp { get; set; }
    public T Body { get; set; }

    public static IEmptyDataStream Empty()
    {
        return new EmptyDataStream();
    }
}