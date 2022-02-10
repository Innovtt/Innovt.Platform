// Company: Antecipa
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests
// Solution: Innovt.Platform
// Date: 2021-07-20

using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors
{
    public class Invoice : BaseInvoice, IDataStream
    {
        public string EventId { get; set; }
        public string Version { get; set; }
        public string Partition { get; set; }
        public string TraceId { get; set; }
        public DateTime ApproximateArrivalTimestamp { get; set; }
    }
}