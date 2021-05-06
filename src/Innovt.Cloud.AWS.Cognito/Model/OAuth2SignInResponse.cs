// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public class OAuth2SignInResponse : SignInResponse
    {
        [JsonPropertyName("error")] public string Error { get; set; }

        public bool NeedRegister { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Picture { get; set; }

        public string Email { get; set; }
    }
}