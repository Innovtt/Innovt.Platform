using Innovt.Core.Serialization;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;


namespace Innovt.Core.Http
{
    public static class HttpConnection
    {
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


        public static HttpRequestDetail Put(Uri endpoint,
            string dataToSend,
            NameValueCollection headerData = null, int? connectionTimeout = null)
        {
            return SendHttpWebRequest(endpoint, HttpMethod.Put.Method, dataToSend, headerData,connectionTimeout);
        }

        public static HttpRequestDetail Delete(Uri endpoint,
            string dataToSend,
            NameValueCollection headerData = null, int? connectionTimeout = null)
        {
            return SendHttpWebRequest(endpoint, HttpMethod.Delete.Method, dataToSend, headerData,
                connectionTimeout);
        }

        public static HttpRequestDetail Head(Uri endpoint,
            string dataToSend,
            NameValueCollection headerData = null, int? connectionTimeout = null)
        {
            return SendHttpWebRequest(endpoint, HttpMethod.Head.Method, dataToSend, headerData,
                connectionTimeout);
        }

        public static HttpRequestDetail Post(Uri endpoint, string dataToSend,
            NameValueCollection headerData = null, int? connectionTimeout = null)
        {
            return SendHttpWebRequest(endpoint, HttpMethod.Post.Method, dataToSend, headerData,
                connectionTimeout);
        }

        private static T DeserializeObject<T>(ISerializer serializer, string content) where T : class
        {
            if (typeof(T).Name.Equals("String")) return (T)Convert.ChangeType(content, typeof(T));

            return serializer.DeserializeObject<T>(content);
        }

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
        internal static HttpRequestDetail SendHttpWebRequest(Uri endpoint, string method, string dataToSend,
            NameValueCollection headerData = null, int? connectionTimeout = null)
        {
            var webRequest = CreateHttpRequest(endpoint, connectionTimeout);
            webRequest.Method = method;
            webRequest.AddHeader(headerData).AddBody(dataToSend);

            var requestDetail = new HttpRequestDetail()
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
}