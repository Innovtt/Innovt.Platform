// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public class RefreshTokenResponse
    {
        [JsonPropertyName("id_token")] public string IdToken { get; set; }

        [JsonPropertyName("access_token")] public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")] public string TokenType { get; set; }
    }
}