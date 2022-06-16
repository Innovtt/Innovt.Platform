// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

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