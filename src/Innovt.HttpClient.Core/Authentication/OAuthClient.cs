using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Utilities;

namespace Innovt.HttpClient.Core.Authentication;

/// <summary>
///     Provides methods for authenticating using OAuth client credentials flow.
/// </summary>
public static class OAuthClient
{
    /// <summary>
    ///     Authenticates using the OAuth client credentials flow.
    /// </summary>
    /// <param name="authUri">The OAuth authorization URI.</param>
    /// <param name="clientId">The client ID for authentication.</param>
    /// <param name="clientSecret">The client secret for authentication.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>An <see cref="OAuthResponse" /> containing authentication details.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="clientId" /> or <paramref name="clientSecret" /> is
    ///     null or empty.
    /// </exception>
    /// <exception cref="HttpRequestException">Thrown if the HTTP request to the authentication URI fails.</exception>
    public static async Task<OAuthResponse> Authenticate(string authUri, string clientId, string clientSecret,
        CancellationToken cancellationToken = default)
    {
        if (clientId.IsNullOrEmpty()) throw new ArgumentNullException(nameof(clientId));
        if (clientSecret.IsNullOrEmpty()) throw new ArgumentNullException(nameof(clientSecret));

        var authorization = "Basic " + $"{clientId}:{clientSecret}".ToBase64(Encoding.UTF8);

        var content = new StringContent("scope=oob&grant_type=client_credentials", Encoding.UTF8,
            BaseConstants.ContentTypeHeaderFormUrlEncoded);

        var httpClient = new System.Net.Http.HttpClient();

        httpClient.DefaultRequestHeaders.Add(BaseConstants.AuthorizationHeader, authorization);

        var response = await httpClient.PostAsync(authUri, content, cancellationToken);

        response.EnsureSuccessStatusCode();

        var contentResponse = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<OAuthResponse>(contentResponse);
    }
}