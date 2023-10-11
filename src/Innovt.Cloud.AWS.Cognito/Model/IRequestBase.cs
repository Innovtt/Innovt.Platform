// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
/// Represents the base interface for request objects with validation support.
/// </summary>
public interface IRequestBase : IValidatableObject
{
#pragma warning disable CA2227 // Collection properties should be read only

    /// <summary>
    /// Gets or sets the collection of HTTP headers associated with the request.
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, string> HttpHeader { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

    /// <summary>
    /// Gets or sets the IP address associated with the request.
    /// </summary>
    [JsonIgnore]
    public string IpAddress { get; set; }

    /// <summary>
    /// Gets or sets the server path associated with the request.
    /// </summary>
    [JsonIgnore]
    public string ServerPath { get; set; }

    /// <summary>
    /// Gets or sets the server name associated with the request.
    /// </summary>
    [JsonIgnore]
    public string ServerName { get; set; }
}