using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Model;

namespace Innovt.Cloud.AWS.Cognito.Tests;

public class SignUpRequest:ISignUpRequest
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return [];
    }

    public Dictionary<string, string> HttpHeader { get; set; }
    public string IpAddress { get; set; }
    public string ServerPath { get; set; }
    public string ServerName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public Dictionary<string, string> CustomAttributes { get; set; }
}