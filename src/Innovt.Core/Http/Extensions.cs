// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace Innovt.Core.Http;

/// <summary>
///     Provides a set of extension methods for working with URIs and HTTP requests.
/// </summary>
public static class Extensions
{
    /// <summary>
    ///     Appends a resource path to the base URI.
    /// </summary>
    /// <param name="baseUri">The base URI to which the resource path will be appended.</param>
    /// <param name="resource">The resource path to append.</param>
    /// <returns>A new URI containing the base URI with the appended resource path.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the baseUri is null.</exception>
    /// <exception cref="Exception">Thrown if the URL cannot be created.</exception>
    public static Uri AppendResourceUri(this Uri baseUri, string resource)
    {
        ArgumentNullException.ThrowIfNull(baseUri);

        if (!Uri.TryCreate(baseUri, resource, out var uniformResourceIdentifier))
            throw new Exception("Cannot create URL; baseURI=" + baseUri + ", resourcePath=" + resource);

        return uniformResourceIdentifier;
    }

    /// <summary>
    ///     Adds headers to an HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request to which headers will be added.</param>
    /// <param name="headerData">The collection of headers to add to the request.</param>
    /// <returns>The modified HTTP request with added headers.</returns>
    public static HttpWebRequest AddHeader(this HttpWebRequest request, NameValueCollection headerData)
    {
        if (headerData == null || request == null)
            return request;


        if (headerData.AllKeys.Contains("User-Agent"))
        {
            request.UserAgent = headerData["User-Agent"];
            headerData.Remove("User-Agent");
        }

        //if (!headerData.AllKeys.Contains("Content-Type"))
        //{
        //    request.ContentType = "application/json";
        //}

        if (headerData.Count > 0)
            foreach (string key in headerData.Keys)
                request.Headers.Add(key, headerData[key]);

        return request;
    }

    /// <summary>
    ///     Adds a request body to an HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request to which the body will be added.</param>
    /// <param name="dataToSend">The data to include in the request body.</param>
    /// <returns>The modified HTTP request with the added request body.</returns>
    public static HttpWebRequest AddBody(this HttpWebRequest request, string dataToSend)
    {
        if (string.IsNullOrWhiteSpace(dataToSend) || request == null)
            return request;


        var byteData = Encoding.UTF8.GetBytes(dataToSend);

        request.ContentLength = byteData.Length;

        using (var writerStream = request.GetRequestStream())
        {
            writerStream.Write(byteData, 0, byteData.Length);
            writerStream.Flush();
        }

        return request;
    }
}