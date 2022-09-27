// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors;

public class InvoiceDomainEvent : DomainEvent
{
    public InvoiceDomainEvent() : base("invoice", "1.0.0")
    {
    }

    public decimal NetValue { get; set; }
}