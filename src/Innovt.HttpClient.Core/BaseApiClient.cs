using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Exceptions;
using Innovt.Core.Serialization;
using Innovt.Core.Utilities;

// ReSharper disable MemberCanBePrivate.Global

namespace Innovt.HttpClient.Core;

/// <summary>
///     Base abstract class for an API client.
/// </summary>
public abstract class BaseApiClient
{
    private readonly IHttpClientFactory httpClientFactory;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BaseApiClient" /> class.
    /// </summary>
    /// <param name="apiContext">The API context associated with this client.</param>
    /// <param name="serializer">The serializer used for data serialization and deserialization.</param>
    protected BaseApiClient(ApiContext apiContext, ISerializer serializer)
    {
        ApiContext = apiContext ?? throw new ArgumentNullException(nameof(apiContext));
        Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    /// <summary>
    ///     This method sends a POST request to the API.
    /// </summary>
    /// <param name="apiContext"></param>
    /// <param name="httpClientFactory"></param>
    /// <param name="serializer"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected BaseApiClient(ApiContext apiContext, IHttpClientFactory httpClientFactory, ISerializer serializer) : this(
        apiContext, serializer)
    {
        this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    /// <summary>
    ///     Gets the API context associated with this client.
    /// </summary>
    protected ApiContext ApiContext { get; }

    /// <summary>
    ///     Gets the serializer used for serializing and deserializing data.
    /// </summary>
    protected ISerializer Serializer { get; }

    /// <summary>
    ///     Creates a new instance of <see cref="System.Net.Http.HttpClient" />.
    /// </summary>
    /// <returns>The created <see cref="System.Net.Http.HttpClient" />.</returns>
    protected virtual System.Net.Http.HttpClient CreateHttpClient()
    {
        return httpClientFactory is not null ? httpClientFactory.CreateClient() : new System.Net.Http.HttpClient();
    }

    /// <summary>
    ///     Parses the HTTP response into a strongly-typed object.
    /// </summary>
    /// <typeparam name="T">The type of the object to parse into.</typeparam>
    /// <param name="response">The HTTP response message.</param>
    /// <returns>The parsed object of type <typeparamref name="T" />.</returns>
    protected virtual async Task<T> ParseResponse<T>(HttpResponseMessage response)
    {
        var contentResponse = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode) return Serializer.DeserializeObject<T>(contentResponse);

        return response.StatusCode switch
        {
            HttpStatusCode.NotFound => default,
            HttpStatusCode.BadRequest => throw new BusinessException(contentResponse),
            _ => throw new HttpRequestException(response.ReasonPhrase)
        };
    }

    /// <summary>
    ///     Parses the HTTP response into a stream.
    /// </summary>
    /// <param name="response">The HTTP response message.</param>
    /// <returns>The response stream.</returns>
    protected virtual async Task<Stream> ParseStreamResponse(HttpResponseMessage response)
    {
        var contentResponse = await response.Content.ReadAsStreamAsync();

        return response.IsSuccessStatusCode ? contentResponse : null;
    }

    /// <summary>
    ///     Sends a POST request and parses the response into a strongly-typed object.
    /// </summary>
    /// <typeparam name="T">The type of the object to parse into.</typeparam>
    /// <param name="baseUri">The base URI for the request.</param>
    /// <param name="resourceUri">The resource URI for the request.</param>
    /// <param name="content">The HTTP content to send.</param>
    /// <param name="headerValues">Additional header values for the request.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The parsed object of type <typeparamref name="T" />.</returns>
    private async Task<T> PostAsync<T>(Uri baseUri, string resourceUri, HttpContent content,
        Dictionary<string, string> headerValues = null, CancellationToken cancellationToken = default)
    {
        var client = CreateHttpClient();

        InitializeClient(client, headerValues);

        var response = await client.PostAsync($"{baseUri}{resourceUri}", content, cancellationToken);

        return await ParseResponse<T>(response);
    }

    /// <summary>
    ///     Asynchronously sends an HTTP POST request to the specified URI with optional headers and content.
    /// </summary>
    /// <param name="baseUri">The base URI for the request.</param>
    /// <param name="resourceUri">The resource URI to append to the base URI.</param>
    /// <param name="content">The HTTP content to send in the request body.</param>
    /// <param name="headerValues">Optional headers to include in the request.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The HTTP status code of the response.</returns>
    private async Task<HttpStatusCode> PostAsync(Uri baseUri, string resourceUri, HttpContent content,
        Dictionary<string, string> headerValues = null, CancellationToken cancellationToken = default)
    {
        var client = CreateHttpClient();

        InitializeClient(client, headerValues);

        var response = await client.PostAsync($"{baseUri}{resourceUri}", content, cancellationToken);

        return response.StatusCode;
    }

    /// <summary>
    ///     Asynchronously sends an HTTP POST request to the specified URI with optional headers and content, and deserializes
    ///     the response to a strongly typed object of type T.
    /// </summary>
    /// <param name="baseUri">The base URI for the request.</param>
    /// <param name="resourceUri">The resource URI to append to the base URI.</param>
    /// <param name="content">The HTTP content to send in the request body.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The strongly typed object of type T representing the response data.</returns>
    protected async Task<T> PostAsync<T>(Uri baseUri, string resourceUri, HttpContent content,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<T>(baseUri, resourceUri, content, null, cancellationToken);
    }

    /// <summary>
    ///     Asynchronously sends an HTTP POST request to the specified URI with content and optional headers.
    /// </summary>
    /// <param name="baseUri">The base URI for the request.</param>
    /// <param name="resourceUri">The resource URI to append to the base URI.</param>
    /// <param name="content">The HTTP content to send in the request body.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The HTTP status code of the response.</returns>
    protected async Task<HttpStatusCode> PostAsync(Uri baseUri, string resourceUri, HttpContent content,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync(baseUri, resourceUri, content, null, cancellationToken);
    }

    /// <summary>
    ///     Asynchronously sends an HTTP PUT request to the specified URI with optional headers and content, and deserializes
    ///     the response to a strongly typed object of type T.
    /// </summary>
    /// <param name="baseUri">The base URI for the request.</param>
    /// <param name="resourceUri">The resource URI to append to the base URI.</param>
    /// <param name="content">The HTTP content to send in the request body.</param>
    /// <param name="headerValues">Optional headers to include in the request.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The strongly typed object of type T representing the response data.</returns>
    protected async Task<T> PutAsync<T>(Uri baseUri, string resourceUri, HttpContent content,
        Dictionary<string, string> headerValues = null, CancellationToken cancellationToken = default)
    {
        var client = CreateHttpClient();

        InitializeClient(client, headerValues);

        var response = await client.PutAsync($"{baseUri}{resourceUri}", content, cancellationToken);

        return await ParseResponse<T>(response);
    }

    /// <summary>
    ///     Asynchronously sends an HTTP PUT request to the specified URI with optional headers and content, and deserializes
    ///     the response to a strongly typed object of type T.
    /// </summary>
    /// <param name="baseUri">The base URI for the request.</param>
    /// <param name="resourceUri">The resource URI to append to the base URI.</param>
    /// <param name="content">The HTTP content to send in the request body.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The strongly typed object of type T representing the response data.</returns>
    protected async Task<T> PutAsync<T>(Uri baseUri, string resourceUri, HttpContent content,
        CancellationToken cancellationToken = default)
    {
        return await PutAsync<T>(baseUri, resourceUri, content, null, cancellationToken);
    }

    /// <summary>
    ///     Initializes the HTTP client with optional headers for authorization and additional custom headers.
    /// </summary>
    /// <param name="client">The HTTP client to initialize.</param>
    /// <param name="headerValues">Optional headers to set on the client.</param>
    private void InitializeClient(System.Net.Http.HttpClient client, Dictionary<string, string> headerValues = null)
    {
        if (ApiContext.AccessToken.IsNotNullOrEmpty() && ApiContext.TokenType.IsNotNullOrEmpty())
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(ApiContext.TokenType, ApiContext.AccessToken);

        if (headerValues == null) return;

        foreach (var (key, value) in headerValues) client.DefaultRequestHeaders.Add(key, value);
    }

    /// <summary>
    ///     Asynchronously sends an HTTP GET request to the specified URI with optional headers and deserializes the response
    ///     to a strongly typed object of type T.
    /// </summary>
    /// <param name="baseUri">The base URI for the request.</param>
    /// <param name="resourceUri">The resource URI to append to the base URI.</param>
    /// <param name="headerValues">Optional headers to include in the request.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The strongly typed object of type T representing the response data.</returns>
    private async Task<T> GetAsync<T>(Uri baseUri, string resourceUri, Dictionary<string, string> headerValues = null,
        CancellationToken cancellationToken = default)
    {
        var client = CreateHttpClient();

        InitializeClient(client, headerValues);

        var response = await client.GetAsync($"{baseUri}{resourceUri}", cancellationToken);

        return await ParseResponse<T>(response);
    }

    /// <summary>
    ///     Asynchronously sends an HTTP GET request to the specified URI with optional headers and deserializes the response
    ///     to a strongly typed object of type T.
    /// </summary>
    /// <param name="baseUri">The base URI for the request.</param>
    /// <param name="resourceUri">The resource URI to append to the base URI.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The strongly typed object of type T representing the response data.</returns>
    protected async Task<T> GetAsync<T>(Uri baseUri, string resourceUri, CancellationToken cancellationToken = default)
    {
        return await GetAsync<T>(baseUri, resourceUri, null, cancellationToken);
    }

    /// <summary>
    ///     Asynchronously sends an HTTP GET request to the specified URI with optional headers and returns the response as a
    ///     stream.
    /// </summary>
    /// <param name="baseUri">The base URI for the request.</param>
    /// <param name="resourceUri">The resource URI to append to the base URI.</param>
    /// <param name="headerValues">Optional headers to include in the request.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A stream representing the response data.</returns>
    private async Task<Stream> GetStreamAsync(Uri baseUri, string resourceUri,
        Dictionary<string, string> headerValues = null, CancellationToken cancellationToken = default)
    {
        var client = CreateHttpClient();

        InitializeClient(client, headerValues);

        var response = await client.GetAsync($"{baseUri}{resourceUri}", cancellationToken);

        return await ParseStreamResponse(response);
    }

    /// <summary>
    ///     Asynchronously sends an HTTP GET request to the specified URI with optional headers and returns the response as a
    ///     stream.
    /// </summary>
    /// <param name="baseUri">The base URI for the request.</param>
    /// <param name="resourceUri">The resource URI to append to the base URI.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A stream representing the response data.</returns>
    protected async Task<Stream> GetStreamAsync(Uri baseUri, string resourceUri,
        CancellationToken cancellationToken = default)
    {
        return await GetStreamAsync(baseUri, resourceUri, null, cancellationToken);
    }
}

/// <summary>
///     Abstract base class for API clients, providing common functionality for sending HTTP requests and handling
///     responses.
/// </summary>
/// <typeparam name="TErrorResponse">Type representing the error response in case of non-successful HTTP requests.</typeparam>
public abstract class BaseApiClient<TErrorResponse> : BaseApiClient where TErrorResponse : class
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BaseApiClient{TErrorResponse}" /> class.
    /// </summary>
    /// <param name="apiContext">The API context containing necessary information like access token and token type.</param>
    /// <param name="httpClientFactory">The factory for creating HTTP clients.</param>
    /// <param name="serializer">The serializer used to deserialize response data.</param>
    protected BaseApiClient(ApiContext apiContext, IHttpClientFactory httpClientFactory, ISerializer serializer) : base(
        apiContext, httpClientFactory, serializer)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BaseApiClient{TErrorResponse}" /> class.
    /// </summary>
    /// <param name="apiContext">The API context containing necessary information like access token and token type.</param>
    /// <param name="serializer">The serializer used to deserialize response data.</param>
    protected BaseApiClient(ApiContext apiContext, ISerializer serializer) : base(apiContext, serializer)
    {
    }

    /// <summary>
    ///     Parses the HTTP response message and handles success or error based on the status code.
    /// </summary>
    /// <typeparam name="T">Type representing the success response.</typeparam>
    /// <param name="response">The HTTP response message to parse.</param>
    /// <returns>The deserialized success response.</returns>
    protected override async Task<T> ParseResponse<T>(HttpResponseMessage response)
    {
        var contentResponse = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode) return Serializer.DeserializeObject<T>(contentResponse);

        var errorResponse = Serializer.DeserializeObject<TErrorResponse>(contentResponse);

        throw new ApiException<TErrorResponse>(errorResponse);
    }

    /// <summary>
    ///     Parses the HTTP response message and handles success or error based on the status code, returning the response as a
    ///     stream.
    /// </summary>
    /// <param name="response">The HTTP response message to parse.</param>
    /// <returns>A stream representing the response data.</returns>
    protected override async Task<Stream> ParseStreamResponse(HttpResponseMessage response)
    {
        var contentResponse = await response.Content.ReadAsStreamAsync();

        if (response.IsSuccessStatusCode) return contentResponse;

        var errorResponse = await response.Content.ReadAsStringAsync();

        throw new ApiException<TErrorResponse>(Serializer.DeserializeObject<TErrorResponse>(errorResponse));
    }
}