// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors;

public class Invoice : BaseInvoice, IDataStream
{
    public string? EventId { get; set; }
    public string Version { get; set; } =null!;
    public string? Partition { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public string? TraceId { get; set; }
    public DateTime ApproximateArrivalTimestamp { get; set; }
}