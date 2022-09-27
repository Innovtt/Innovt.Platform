// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System;

namespace Innovt.Domain.Core.Streams;

internal class EmptyDataStream : IEmptyDataStream
{
    public EmptyDataStream()
    {
        Version = "1.0.0";
    }

    public string Version { get; set; }
    public string EventId { get; set; }
    public string Partition { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public string TraceId { get; set; }
    public DateTime ApproximateArrivalTimestamp { get; set; }
}