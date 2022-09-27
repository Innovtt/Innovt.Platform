// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using Innovt.Core.Exceptions;

namespace Innovt.Core.Http;

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