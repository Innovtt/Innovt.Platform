// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Core.Exceptions;

namespace Innovt.Core.Http
{
    public class HttpException : BaseException
    {
        public HttpException(HttpRequestDetail detail)
        {
            RequestDetail = detail;
        }

        public HttpException(Exception requestException)
        {
            RequestException = requestException;
        }

        public Exception RequestException { get; }
        public HttpRequestDetail RequestDetail { get; }
    }
}