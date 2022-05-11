// Company: Antecipa
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests
// Solution: Innovt.Platform
// Date: 2021-07-20

using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors;

public class InvoiceDomainEvent : DomainEvent
{
    public InvoiceDomainEvent() : base("invoice", "1.0.0")
    {
    }

    public decimal NetValue { get; set; }
}