// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Cognito.Model;

public interface IRequestBase : IValidatableObject
{
#pragma warning disable CA2227 // Collection properties should be read only
    [JsonIgnore] public Dictionary<string, string> HttpHeader { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

    [JsonIgnore] public string IpAddress { get; set; }

    [JsonIgnore] public string ServerPath { get; set; }

    [JsonIgnore] public string ServerName { get; set; }
}