﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Cognito.Model;

public class AuthChallengeResponse
{
    public IDictionary<string, string> Metadata { get; set; }

    public SignInResponse AuthenticationResult { get; set; }
}