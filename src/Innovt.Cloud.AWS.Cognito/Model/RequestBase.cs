// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Base class for request objects used in API calls.
/// </summary>
public abstract class RequestBase : IRequestBase
{
    /// <summary>
    ///     Gets or sets the HTTP headers associated with the request.
    /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
    [JsonIgnore]
    public Dictionary<string, string> HttpHeader { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

    /// <summary>
    ///     Gets or sets the IP address of the requester.
    /// </summary>
    [JsonIgnore]
    public string IpAddress { get; set; }

    /// <summary>
    ///     Gets or sets the server path for the request.
    /// </summary>
    [JsonIgnore]
    public string ServerPath { get; set; }

    /// <summary>
    ///     Gets or sets the server name for the request.
    /// </summary>
    [JsonIgnore]
    public string ServerName { get; set; }

    /// <summary>
    ///     Validates the request object.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection of validation results.</returns>
    public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
}