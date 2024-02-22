// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System;

namespace Innovt.Domain.Core.Streams;

/// <summary>
///     Represents an empty data stream with no body.
/// </summary>
internal class EmptyDataStream : IEmptyDataStream
{
    /// <summary>
    ///     Initializes a new instance of the EmptyDataStream class with default properties.
    /// </summary>
    public EmptyDataStream()
    {
        Version = "1.0.0";
    }

    /// <summary>
    ///     Gets or sets the version of the empty data stream.
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    ///     Gets or sets the event identifier associated with the empty data stream.
    /// </summary>
    public string EventId { get; set; }

    /// <summary>
    ///     Gets or sets the partition identifier of the empty data stream.
    /// </summary>
    public string Partition { get; set; }

    /// <summary>
    ///     Gets or sets the date and time when the empty data stream was published.
    /// </summary>
    public DateTimeOffset? PublishedAt { get; set; }

    /// <summary>
    ///     Gets or sets the trace identifier associated with the empty data stream.
    /// </summary>
    public string TraceId { get; set; }

    /// <summary>
    ///     Gets or sets the approximate arrival timestamp of the empty data stream.
    /// </summary>
    public DateTime ApproximateArrivalTimestamp { get; set; }
}