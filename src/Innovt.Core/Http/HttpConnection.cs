// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using Innovt.Core.Serialization;

namespace Innovt.Core.Http;

/// <summary>
///     Provides methods for making HTTP requests and handling responses.
/// </summary>
public static class HttpConnection
{
    /// <summary>
    ///     Creates an HTTP request with the specified URL and optional connection timeout.
    /// </summary>
    /// <param name="url">The URL to create the request for.</param>
    /// <param name="connectionTimeout">The connection timeout in milliseconds (default is 30000 ms).</param>
    /// <returns>An <see cref="HttpWebRequest" /> instance.</returns>
    private static HttpWebRequest CreateHttpRequest(Uri url, int? connectionTimeout = 30000)
    {
        var httpRequest = (HttpWebRequest)WebRequest.Create(url);

        httpRequest.Timeout = connectionTimeout ?? 30000;

        // Don't set the Expect: 100-continue header as it's not supported
        // well by Akamai and can negatively impact performance.
        httpRequest.ServicePoint.Expect100Continue = false;
        httpRequest.AutomaticDecompression = DecompressionMethods.GZip;

        return httpRequest;
    }

    //public static HttpRequestDetail Get(Uri endpoint,
    //    string dataToSend,
    //    NameValueCollection headerData = null, int? connectionTimeout = null)
    //{
    //    return SendHttpWebRequest(endpoint, HttpMethod.Get.Method, dataToSend, headerData,
    //        connectionTimeout);
    //}

    /// <summary>
    ///     Sends an HTTP GET request to the specified endpoint, deserializes the response to the specified type,
    ///     and returns the deserialized object.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The URI of the HTTP endpoint.</param>
    /// <param name="serializer">An instance of the serializer to use for deserialization.</param>
    /// <param name="dataToSend">Optional data to send with the request.</param>
    /// <param name="headerData">Optional headers to include in the request.</param>
    /// <param name="connectionTimeout">Optional connection timeout in milliseconds.</param>
    /// <returns>The deserialized response of type <typeparamref name="T" />.</returns>
    /// <exception cref="HttpException">Thrown if the response status code is not OK or Created.</exception>
    public static T Get<T>(Uri endpoint, ISerializer serializer,
        string dataToSend = null,
        NameValueCollection headerData = null, int? connectionTimeout = null) where T : class
    {
        var response = SendHttpWebRequest(endpoint, HttpMethod.Get.Method, dataToSend, headerData,
            connectionTimeout);

        if (response.ResponseStatusCode != HttpStatusCode.OK &&
            response.ResponseStatusCode != HttpStatusCode.Created)
            throw new HttpException(response);

        return DeserializeObject<T>(serializer, response.RawResponse);
    }


    /// <summary>
    ///     Sends an HTTP PUT request to the specified endpoint and retrieves detailed information about the request and
    ///     response.
    /// </summary>
    /// <param name="endpoint">The URI of the HTTP endpoint.</param>
    /// <param name="dataToSend">Optional data to send with the request.</param>
    /// <param name="headerData">Optional headers to include in the request.</param>
    /// <param name="connectionTimeout">Optional connection timeout in milliseconds.</param>
    /// <returns>An <see cref="HttpRequestDetail" /> object containing request and response details.</returns>
    public static HttpRequestDetail Put(Uri endpoint,
        string dataToSend,
        NameValueCollection headerData = null, int? connectionTimeout = null)
    {
        return SendHttpWebRequest(endpoint, HttpMethod.Put.Method, dataToSend, headerData, connectionTimeout);
    }

    /// <summary>
    ///     Sends an HTTP DELETE request to the specified endpoint and retrieves detailed information about the request and
    ///     response.
    /// </summary>
    /// <param name="endpoint">The URI of the HTTP endpoint.</param>
    /// <param name="dataToSend">Optional data to send with the request.</param>
    /// <param name="headerData">Optional headers to include in the request.</param>
    /// <param name="connectionTimeout">Optional connection timeout in milliseconds.</param>
    /// <returns>An <see cref="HttpRequestDetail" /> object containing request and response details.</returns>
    public static HttpRequestDetail Delete(Uri endpoint,
        string dataToSend,
        NameValueCollection headerData = null, int? connectionTimeout = null)
    {
        return SendHttpWebRequest(endpoint, HttpMethod.Delete.Method, dataToSend, headerData,
            connectionTimeout);
    }

    /// <summary>
    ///     Sends an HTTP HEAD request to the specified endpoint and retrieves detailed information about the request and
    ///     response.
    /// </summary>
    /// <param name="endpoint">The URI of the HTTP endpoint.</param>
    /// <param name="dataToSend">Optional data to send with the request.</param>
    /// <param name="headerData">Optional headers to include in the request.</param>
    /// <param name="connectionTimeout">Optional connection timeout in milliseconds.</param>
    /// <returns>An <see cref="HttpRequestDetail" /> object containing request and response details.</returns>
    public static HttpRequestDetail Head(Uri endpoint,
        string dataToSend,
        NameValueCollection headerData = null, int? connectionTimeout = null)
    {
        return SendHttpWebRequest(endpoint, HttpMethod.Head.Method, dataToSend, headerData,
            connectionTimeout);
    }


    /// <summary>
    ///     Sends an HTTP POST request to the specified endpoint and retrieves detailed information about the request and
    ///     response.
    /// </summary>
    /// <param name="endpoint">The URI of the HTTP endpoint.</param>
    /// <param name="dataToSend">The data to send with the request.</param>
    /// <param name="headerData">Optional headers to include in the request.</param>
    /// <param name="connectionTimeout">Optional connection timeout in milliseconds.</param>
    /// <returns>An <see cref="HttpRequestDetail" /> object containing request and response details.</returns>
    public static HttpRequestDetail Post(Uri endpoint, string dataToSend,
        NameValueCollection headerData = null, int? connectionTimeout = null)
    {
        return SendHttpWebRequest(endpoint, HttpMethod.Post.Method, dataToSend, headerData,
            connectionTimeout);
    }

    /// <summary>
    ///     Deserializes the HTTP response content to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to.</typeparam>
    /// <param name="serializer">An instance of the serializer to use for deserialization.</param>
    /// <param name="content">The response content to deserialize.</param>
    /// <returns>The deserialized object of type <typeparamref name="T" />.</returns>
    private static T DeserializeObject<T>(ISerializer serializer, string content) where T : class
    {
        if (typeof(T).Name.Equals("String")) return (T)Convert.ChangeType(content, typeof(T));

        return serializer.DeserializeObject<T>(content);
    }

    /// <summary>
    ///     Sends an HTTP POST request to the specified endpoint and retrieves the response, deserializing it to the specified
    ///     type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The URI of the HTTP endpoint.</param>
    /// <param name="dataToSend">The data to send with the request.</param>
    /// <param name="serializer">An instance of the serializer to use for deserialization.</param>
    /// <param name="headerData">Optional headers to include in the request.</param>
    /// <param name="connectionTimeout">Optional connection timeout in milliseconds.</param>
    /// <returns>The deserialized response of type <typeparamref name="T" />.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the serializer is null.</exception>
    /// <exception cref="HttpException">Thrown if the response status code is not OK or Created.</exception>
    public static T Post<T>(Uri endpoint, string dataToSend, ISerializer serializer,
        NameValueCollection headerData = null, int? connectionTimeout = null) where T : class
    {
        if (serializer == null) throw new ArgumentNullException(nameof(serializer));

        var response = SendHttpWebRequest(endpoint, HttpMethod.Post.Method, dataToSend, headerData,
            connectionTimeout);

        if (response.ResponseStatusCode != HttpStatusCode.OK &&
            response.ResponseStatusCode != HttpStatusCode.Created)
            throw new HttpException(response);

        return DeserializeObject<T>(serializer, response.RawResponse);
    }

    //PostAsync
    /// <summary>
    ///     Sends an HTTP request to the specified endpoint and retrieves detailed information about the request and response.
    /// </summary>
    /// <param name="endpoint">The URI of the HTTP endpoint.</param>
    /// <param name="method">The HTTP method to use (e.g., GET, POST, PUT).</param>
    /// <param name="dataToSend">Optional data to send with the request.</param>
    /// <param name="headerData">Optional headers to include in the request.</param>
    /// <param name="connectionTimeout">Optional connection timeout in milliseconds.</param>
    /// <returns>An <see cref="HttpRequestDetail" /> object containing request and response details.</returns>
    internal static HttpRequestDetail SendHttpWebRequest(Uri endpoint, string method, string dataToSend,
        NameValueCollection headerData = null, int? connectionTimeout = null)
    {
        var webRequest = CreateHttpRequest(endpoint, connectionTimeout);
        webRequest.Method = method;
        webRequest.AddHeader(headerData).AddBody(dataToSend);

        var requestDetail = new HttpRequestDetail
        {
            Url = webRequest.RequestUri.AbsoluteUri,
            Method = webRequest.Method,
            Header = headerData,
            RawRequest = dataToSend
        };

        try
        {
            using var webResponse = webRequest.GetResponse() as HttpWebResponse;
            var streamReader = new StreamReader(webResponse.GetResponseStream());
            requestDetail.RawResponse = streamReader.ReadToEnd();
            requestDetail.ResponseStatusCode = webResponse.StatusCode;
        }
        catch (WebException ex)
        {
            if (ex.Response == null) throw;

            using var webResponse = (HttpWebResponse)ex.Response;
            var test = new StreamReader(webResponse.GetResponseStream());
            requestDetail.RawResponse = test.ReadToEnd();
            requestDetail.ResponseStatusCode = webResponse.StatusCode;
        }


        return requestDetail;
    }
}