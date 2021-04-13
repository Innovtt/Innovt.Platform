// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Tracking
{
    public class RequestTracking : ValueObject<Guid>
    {
        public RequestTracking()
        {
            Id = Guid.NewGuid();
        }

        public string UserId { get; set; }

        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Verb { get; set; }

        public string Host { get; set; }

        public int? ResponseStatusCode { get; set; }
    }
}