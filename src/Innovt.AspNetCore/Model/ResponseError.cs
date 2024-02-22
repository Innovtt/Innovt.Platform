// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

namespace Innovt.AspNetCore.Model;

/// <summary>
///     Represents an error response object returned by the API.
/// </summary>
public class ResponseError
{
    /// <summary>
    ///     Gets or sets a unique identifier for tracing the error.
    /// </summary>
    public string? TraceId { get; set; }

    /// <summary>
    ///     Gets or sets a message describing the error.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    ///     Gets or sets an error code associated with the error (optional).
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    ///     Gets or sets additional error details.
    /// </summary>
    public object? Detail { get; set; }
}