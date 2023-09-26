// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System;
using Innovt.Domain.Core.Streams;

namespace Innovt.Domain.Core.Events;
/// <summary>
/// Represents a domain event in the system.
/// </summary>
public abstract class DomainEvent : IDataStream
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEvent"/> class with a specified name, version, and partition.
    /// </summary>
    /// <param name="name">The name of the domain event.</param>
    /// <param name="version">The version of the domain event.</param>
    /// <param name="partition">The partition associated with the event.</param>
    protected DomainEvent(string name, string version, string partition)
    {
        Name = name;
        Version = version;
        Partition = partition;
        CreatedAt = DateTime.UtcNow;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEvent"/> class with a specified name and partition.
    /// </summary>
    /// <param name="name">The name of the domain event.</param>
    /// <param name="partition">The partition associated with the event.</param>
    protected DomainEvent(string name, string partition)
    {
        Name = name;
        Partition = partition;
        CreatedAt = DateTime.UtcNow;
    }
    /// <summary>
    /// Gets or sets the name of the domain event.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Gets or sets the date and time when the domain event was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the event ID associated with the domain event.
    /// </summary>
    public string EventId { get; set; }
    /// <summary>
    /// Gets or sets the version of the domain event.
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// Gets or sets the partition associated with the domain event.
    /// </summary>
    public string Partition { get; set; }
    /// <summary>
    /// Gets or sets the date and time when the domain event was published.
    /// </summary>
    public DateTimeOffset? PublishedAt { get; set; }
    /// <summary>
    /// Gets or sets the trace ID associated with the domain event.
    /// </summary>
    public string TraceId { get; set; }
    /// <summary>
    /// Gets or sets the approximate arrival timestamp of the domain event.
    /// </summary>
    public DateTime ApproximateArrivalTimestamp { get; set; }
    /// <summary>
    /// Creates an empty domain event with the specified partition.
    /// </summary>
    /// <param name="partition">The partition associated with the event.</param>
    /// <returns>An instance of <see cref="DomainEvent"/> representing an empty domain event.</returns>
    public static DomainEvent Empty(string partition)
    {
        return new EmptyDomainEvent(partition);
    }
}