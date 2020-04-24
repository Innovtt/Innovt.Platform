using System;

namespace Innovt.Domain.Model.Tracking
{
    public class RequestTracking:ValueObject<Guid>
    {
        public string UserId { get; set; }

        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Verb { get; set; }

        public string Host { get; set; }

        public int? ResponseStatusCode { get; set; }

        public RequestTracking()
        {
            Id = Guid.NewGuid();
        }
    }
}
