using System;
using System.Collections.Specialized;
using System.Net;

namespace Innovt.Core.Http
{
    public class HttpRequestDetail
    {
        public Guid Id { get; set; }

        public string Url { get; set; }
        public string Method { get; set; }
        public NameValueCollection Header { get; set; }

        public string RawRequest { get; set; }
        public string RawResponse { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; }
      

        public HttpRequestDetail()
        {
            Id = Guid.NewGuid();
        }
    }
}