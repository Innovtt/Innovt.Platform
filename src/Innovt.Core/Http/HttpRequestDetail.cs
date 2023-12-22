// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Specialized;
using System.Net;

namespace Innovt.Core.Http;

/// <summary>
/// Represents detailed information about an HTTP request, including its identifier, URL, method, headers, request content,
/// response content, and response status code.
/// </summary>
public class HttpRequestDetail
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRequestDetail"/> class with a unique identifier.
    /// </summary>
    public HttpRequestDetail()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Gets or sets the unique identifier for the HTTP request detail.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the URL of the HTTP request.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the HTTP method used in the request (e.g., GET, POST, PUT).
    /// </summary>
    public string Method { get; set; }

    /// <summary>
    /// Gets or sets the collection of headers associated with the HTTP request.
    /// </summary>
    public NameValueCollection Header { get; set; }

    /// <summary>
    /// Gets or sets the raw content of the HTTP request.
    /// </summary>
    public string RawRequest { get; set; }

    /// <summary>
    /// Gets or sets the raw content of the HTTP response.
    /// </summary>
    public string RawResponse { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code of the response.
    /// </summary>
    public HttpStatusCode ResponseStatusCode { get; set; }
}