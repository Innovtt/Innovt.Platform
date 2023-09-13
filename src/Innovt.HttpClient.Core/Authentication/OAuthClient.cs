using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Utilities;

namespace Innovt.HttpClient.Core.Authentication
{
    public static class OAuthClient
    {
        public static async Task<OAuthResponse> Authenticate(string authUri, string clientId, string clientSecret, CancellationToken cancellationToken = default)
        {
            if (clientId.IsNullOrEmpty()) throw new ArgumentNullException(nameof(clientId));
            if (clientSecret.IsNullOrEmpty()) throw new ArgumentNullException(nameof(clientSecret));

            var authorization = "Basic " + $"{clientId}:{clientSecret}".ToBase64(Encoding.UTF8);
            
            var content = new StringContent("scope=oob&grant_type=client_credentials", Encoding.UTF8, BaseConstants.ContentTypeHeaderFormUrlEncoded);

            var httpClient = new System.Net.Http.HttpClient();

            httpClient.DefaultRequestHeaders.Add(BaseConstants.AuthorizationHeader, authorization);

            var response = await httpClient.PostAsync(authUri, content, cancellationToken);

            response.EnsureSuccessStatusCode();

            var contentResponse = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<OAuthResponse>(contentResponse);
        }
    }
}