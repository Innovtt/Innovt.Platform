// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System;

namespace Innovt.Domain.Core.Streams;

/// <summary>
/// Represents a data stream without a specific body type.
/// </summary>
public interface IDataStream
{
    /// <summary>
    /// Gets or sets the event identifier associated with the data stream.
    /// </summary>
    public string EventId { get; set; }

    /// <summary>
    /// Gets or sets the version of the data stream.
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets the partition identifier of the data stream.
    /// </summary>
    public string Partition { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the data stream was published.
    /// </summary>
    public DateTimeOffset? PublishedAt { get; set; }

    /// <summary>
    /// Gets or sets the trace identifier associated with the data stream.
    /// </summary>
    public string TraceId { get; set; }

    /// <summary>
    /// Gets or sets the approximate arrival timestamp of the data stream.
    /// </summary>
    public DateTime ApproximateArrivalTimestamp { get; set; }
}

/// <summary>
/// Represents a data stream with a specific body type.
/// </summary>
/// <typeparam name="T">The type of the body of the data stream.</typeparam>
public interface IDataStream<T> : IDataStream where T : class
{
    /// <summary>
    /// Gets or sets the body of the data stream.
    /// </summary>
    public T Body { get; set; }
}