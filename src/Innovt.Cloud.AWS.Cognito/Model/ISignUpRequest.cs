// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Cognito.Model;

public interface ISignUpRequest : IRequestBase
{
    string UserName { get; set; }

    string Password { get; set; }

    public Dictionary<string, string> CustomAttributes { get; set; }
}