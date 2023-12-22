// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Tracking;

/// <summary>
///     Represents a request tracking record for monitoring HTTP requests.
/// </summary>
public class RequestTracking : ValueObject<Guid>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RequestTracking" /> class.
    /// </summary>
    public RequestTracking()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    ///     Gets or sets the user ID associated with the request.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    ///     Gets or sets the area associated with the request.
    /// </summary>
    public string Area { get; set; }

    /// <summary>
    ///     Gets or sets the controller associated with the request.
    /// </summary>
    public string Controller { get; set; }

    /// <summary>
    ///     Gets or sets the action associated with the request.
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    ///     Gets or sets the HTTP verb used in the request.
    /// </summary>
    public string Verb { get; set; }

    /// <summary>
    ///     Gets or sets the host associated with the request.
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    ///     Gets or sets the HTTP response status code.
    /// </summary>
    public int? ResponseStatusCode { get; set; }
}