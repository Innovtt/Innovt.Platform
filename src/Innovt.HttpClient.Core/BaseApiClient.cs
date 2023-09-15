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
// ReSharper disable MemberCanBePrivate.Global

namespace Innovt.HttpClient.Core
{
    public abstract class BaseApiClient
    {
        private readonly IHttpClientFactory httpClientFactory;

        protected ApiContext ApiContext { get; }

        protected  ISerializer Serializer { get; }

        protected BaseApiClient(ApiContext apiContext, ISerializer serializer)
        {
            this.ApiContext = apiContext ?? throw new ArgumentNullException(nameof(apiContext));
            this.Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }
        
        protected BaseApiClient(ApiContext apiContext, IHttpClientFactory httpClientFactory,ISerializer serializer):this(apiContext,serializer)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
       
        protected virtual System.Net.Http.HttpClient CreateHttpClient()
        {
            return httpClientFactory is not null ? httpClientFactory.CreateClient() : new System.Net.Http.HttpClient();
        }

        protected virtual async Task<T> ParseResponse<T>(HttpResponseMessage response)
        {
            var contentResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return Serializer.DeserializeObject<T>(contentResponse);
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return default(T);

                case HttpStatusCode.BadRequest:
                    throw new BusinessException(contentResponse);
                default:
                    throw new HttpRequestException(response.ReasonPhrase);
            }
        }

        protected virtual async Task<Stream> ParseStreamResponse(HttpResponseMessage response)
        {
            var contentResponse = await response.Content.ReadAsStreamAsync();

            return response.IsSuccessStatusCode ? contentResponse : null;
        }

        private async Task<T> PostAsync<T>(Uri baseUri, string resourceUri, HttpContent content,Dictionary<string,string> headerValues=null, CancellationToken cancellationToken = default)
        {
            var client = CreateHttpClient();

            InitializeClient(client,headerValues);

            var response = await client.PostAsync($"{baseUri.ToString()}{resourceUri}", content, cancellationToken);

            return await ParseResponse<T>(response);
        }

        protected async Task<T> PostAsync<T>(Uri baseUri, string resourceUri, HttpContent content, CancellationToken cancellationToken = default)
        {
            return await PostAsync<T>(baseUri, resourceUri, content, null, cancellationToken);
        }

        protected async Task<T> PutAsync<T>(Uri baseUri, string resourceUri, HttpContent content,Dictionary<string,string> headerValues=null, CancellationToken cancellationToken = default)
        {
            var client = CreateHttpClient();

            InitializeClient(client,headerValues);

            var response = await client.PutAsync($"{baseUri.ToString()}{resourceUri}", content, cancellationToken);

            return await ParseResponse<T>(response);
        }

        protected async Task<T> PutAsync<T>(Uri baseUri, string resourceUri, HttpContent content, CancellationToken cancellationToken = default)
        {
            return await PutAsync<T>(baseUri, resourceUri, content, null, cancellationToken);
        }

        private void InitializeClient(System.Net.Http.HttpClient client, Dictionary<string, string> headerValues = null)
        {  
            if (ApiContext.AccessToken != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ApiContext.TokenType, ApiContext.AccessToken);
            }

            if (headerValues == null) return;

            foreach (var (key, value) in headerValues)
            {
                client.DefaultRequestHeaders.Add(key, value);
            }
        }

        private async Task<T> GetAsync<T>(Uri baseUri, string resourceUri, Dictionary<string, string> headerValues = null, CancellationToken cancellationToken = default)
        {
            var client = CreateHttpClient();

            InitializeClient(client,headerValues);

            var response = await client.GetAsync($"{baseUri.ToString()}{resourceUri}", cancellationToken);

            return await ParseResponse<T>(response);
        }

        protected async Task<T> GetAsync<T>(Uri baseUri, string resourceUri, CancellationToken cancellationToken = default)
        {
            return await GetAsync<T>(baseUri,resourceUri,null,cancellationToken);
        }

        private async Task<Stream> GetStreamAsync(Uri baseUri, string resourceUri, Dictionary<string, string> headerValues = null, CancellationToken cancellationToken = default)
        {
            var client = CreateHttpClient();

            InitializeClient(client,headerValues);

            var response = await client.GetAsync($"{baseUri.ToString()}{resourceUri}", cancellationToken);

            return await ParseStreamResponse(response);
        }

        protected async Task<Stream> GetStreamAsync(Uri baseUri, string resourceUri,
             CancellationToken cancellationToken = default)
        {
            return await GetStreamAsync(baseUri, resourceUri, null, cancellationToken);
        }
    }
    
    public abstract class BaseApiClient<TErrorResponse>: BaseApiClient where TErrorResponse:class
    { 
        protected BaseApiClient(ApiContext apiContext, IHttpClientFactory httpClientFactory,ISerializer serializer) : base(apiContext, httpClientFactory,serializer)
        {
        }
        
        protected override async Task<T> ParseResponse<T>(HttpResponseMessage response)
        {
            var contentResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return Serializer.DeserializeObject<T>(contentResponse);
            }
            else
            {
                var errorResponse = Serializer.DeserializeObject<TErrorResponse>(contentResponse); 

                throw  new ApiException<TErrorResponse>(errorResponse);
            }
        }

        protected override async Task<Stream> ParseStreamResponse(HttpResponseMessage response)
        {
            var contentResponse = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                return contentResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                
                throw  new ApiException<TErrorResponse>(Serializer.DeserializeObject<TErrorResponse>(errorResponse));
            }
        }

      
    }
}