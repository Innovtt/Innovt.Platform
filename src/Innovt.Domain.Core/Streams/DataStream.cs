// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System;

namespace Innovt.Domain.Core.Streams;

/// <summary>
///     Represents a data stream with a specific type of body.
/// </summary>
/// <typeparam name="T">The type of the body.</typeparam>
public class DataStream<T> : IDataStream<T> where T : class
{
    /// <summary>
    ///     Default constructor for DataStream.
    /// </summary>
    public DataStream()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the DataStream class with specified properties.
    /// </summary>
    /// <param name="version">The version of the data stream.</param>
    /// <param name="partition">The partition identifier of the data stream.</param>
    /// <param name="traceId">The trace identifier associated with the data stream.</param>
    /// <param name="body">The body of the data stream.</param>
    public DataStream(string version, string? partition, string? traceId, T body)
    {
        Version = version;
        Partition = partition;
        TraceId = traceId;
        Body = body;
    }

    /// <summary>
    ///     Gets or sets the version of the data stream.
    /// </summary>
    public string Version { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the event identifier associated with the data stream.
    /// </summary>
    public string? EventId { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the partition identifier of the data stream.
    /// </summary>
    public string? Partition { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the date and time when the data stream was published.
    /// </summary>
    public DateTimeOffset? PublishedAt { get; set; }

    /// <summary>
    ///     Gets or sets the trace identifier associated with the data stream.
    /// </summary>
    public string? TraceId { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the approximate arrival timestamp of the data stream.
    /// </summary>
    public DateTime ApproximateArrivalTimestamp { get; set; }

    /// <summary>
    ///     Gets or sets the body of the data stream.
    /// </summary>
    public T Body { get; set; } = null!;

    /// <summary>
    ///     Creates an empty data stream with no body.
    /// </summary>
    /// <returns>An instance of an empty data stream.</returns>
    public static IEmptyDataStream Empty()
    {
        return new EmptyDataStream();
    }
}