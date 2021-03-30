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
        public Dictionary<string, string> ChallengeParameters { get; set; }
    }
}