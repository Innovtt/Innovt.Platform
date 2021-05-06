// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Specialized;
using System.Net;

namespace Innovt.Core.Http
{
    public class HttpRequestDetail
    {
        public HttpRequestDetail()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Url { get; set; }
        public string Method { get; set; }
        public NameValueCollection Header { get; set; }

        public string RawRequest { get; set; }
        public string RawResponse { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; }
    }
}