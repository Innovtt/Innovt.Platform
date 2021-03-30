using System;
using Innovt.Core.Exceptions;

namespace Innovt.Core.Http
{
    public class HttpException : BaseException
    {
        public Exception RequestException { get; }
        public HttpRequestDetail RequestDetail { get; private set; }

        public HttpException(HttpRequestDetail detail)
        {
            RequestDetail = detail;
        }

        public HttpException(Exception requestException)
        {
            RequestException = requestException;
        }
    }
}