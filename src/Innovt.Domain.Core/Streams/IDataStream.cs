// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System;

namespace Innovt.Domain.Core.Streams;

public interface IDataStream
{
    public string EventId { get; set; }

    public string Version { get; set; }

    public string Partition { get; set; }

    public DateTimeOffset? PublishedAt { get; set; }
    public string TraceId { get; set; }
    public DateTime ApproximateArrivalTimestamp { get; set; }
}

public interface IDataStream<T> : IDataStream where T : class
{
    public T Body { get; set; }
}