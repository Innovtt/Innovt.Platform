// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using Innovt.Core.Exceptions;

namespace Innovt.Core.Http;

/// <summary>
///     Represents an exception related to HTTP requests, providing information about the request details and any
///     associated exception.
/// </summary>
public class HttpException : BaseException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="HttpException" /> class with information about the HTTP request.
    /// </summary>
    /// <param name="detail">The details of the HTTP request that triggered the exception.</param>
    public HttpException(HttpRequestDetail detail)
    {
        RequestDetail = detail;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="HttpException" /> class with an exception related to the HTTP request.
    /// </summary>
    /// <param name="requestException">The exception related to the HTTP request.</param>
    public HttpException(Exception requestException)
    {
        RequestException = requestException;
    }

    /// <summary>
    ///     Gets the exception related to the HTTP request.
    /// </summary>
    public Exception RequestException { get; }

    /// <summary>
    ///     Gets the details of the HTTP request that triggered the exception.
    /// </summary>
    public HttpRequestDetail RequestDetail { get; }
}