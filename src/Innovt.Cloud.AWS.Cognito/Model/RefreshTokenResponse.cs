// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Cognito.Model;

public class RefreshTokenResponse
{
    [JsonPropertyName("id_token")] public string IdToken { get; set; }

    [JsonPropertyName("access_token")] public string AccessToken { get; set; }

    [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }

    [JsonPropertyName("token_type")] public string TokenType { get; set; }
}