// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhaes
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;

namespace Innovt.Domain.Core.Streams
{
    internal class EmptyDataStream : IEmptyDataStream
    {
        public EmptyDataStream()
        {
            Version = "1.0.0";
        }
        public string Version { get; set; }
        public string EventId { get; set; }
        public string Partition { get; set; }
        public string TraceId { get; set; }
        public DateTime ApproximateArrivalTimestamp { get; set; }
    }
}