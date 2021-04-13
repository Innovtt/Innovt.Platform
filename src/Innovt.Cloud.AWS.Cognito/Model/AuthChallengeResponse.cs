// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public class AuthChallengeResponse
    {
        public IDictionary<string, string> Metadata { get; set; }

        public SignInResponse AuthenticationResult { get; set; }
    }
}