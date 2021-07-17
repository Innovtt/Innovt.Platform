// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public class SignInResponse
    {
        [JsonPropertyName("id_token")] public string IdToken { get; set; }

        [JsonPropertyName("access_token")] public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; }

        [JsonPropertyName("token_type")] public string TokenType { get; set; }

        [JsonPropertyName("signInType")] public string SignInType { get; set; }

        [JsonPropertyName("session")] public string Session { get; set; }

        [JsonPropertyName("ChallengeName")] public string ChallengeName { get; set; }

        [JsonPropertyName("ChallengeParameters")]
#pragma warning disable CA2227 // Collection properties should be read only
        public Dictionary<string, string> ChallengeParameters { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}