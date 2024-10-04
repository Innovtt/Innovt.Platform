using System.Text.Json.Serialization;

namespace Innovt.AspNetCore.Filters.Recaptcha;

/// <summary>
///     Represents the response received from reCAPTCHA verification.
/// </summary>
internal class RecaptchaResponse
{
    /// <summary>
    ///     Indicates whether the reCAPTCHA verification was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    ///     The score obtained from the reCAPTCHA verification.
    /// </summary>
    [JsonPropertyName("score")]
    public decimal Score { get; set; }

    /// <summary>
    ///     The action associated with the reCAPTCHA verification.
    /// </summary>
    [JsonPropertyName("action")]
    public string? Action { get; set; }

    /// <summary>
    ///     The timestamp of the challenge.
    /// </summary>
    [JsonPropertyName("challenge_ts")]
    public string? ChallengeTs { get; set; }

    /// <summary>
    ///     The hostname from which the reCAPTCHA verification originated.
    /// </summary>
    [JsonPropertyName("hostname")]
    public string? HostName { get; set; }
}