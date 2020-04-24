using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;

namespace Innovt.Core.Http
{
    public static class Extensions
    {
        public static Uri AppendResourceUri(this Uri baseUri, string resource)
        {
            if (baseUri == null) throw new ArgumentNullException(nameof(baseUri));

            if (!Uri.TryCreate(baseUri, resource, out var uniformResourceIdentifier))
            {
                throw new Exception("Cannot create URL; baseURI=" + baseUri + ", resourcePath=" + resource);
            }

            return uniformResourceIdentifier;
        }

        public static HttpWebRequest AddHeader(this HttpWebRequest request, NameValueCollection headerData)
        {
            if (headerData == null || request==null)
                return request;

           
            if (headerData.AllKeys.Contains("User-Agent") == true)
            {
                request.UserAgent = headerData["User-Agent"];
                headerData.Remove("User-Agent");
            }

            //if (!headerData.AllKeys.Contains("Content-Type"))
            //{
            //    request.ContentType = "application/json";
            //}

            if (headerData.Count > 0)
            {
                foreach (string key in headerData.Keys)
                {
                    request.Headers.Add(key, headerData[key].ToString());
                }
            }

            return request;
        }

        public static HttpWebRequest AddBody(this HttpWebRequest request, string dataToSend)
        {
            if (string.IsNullOrWhiteSpace(dataToSend) || request == null)
                return request;


            byte[] byteData = UTF8Encoding.UTF8.GetBytes(dataToSend);
            
            request.ContentLength = byteData.Length;

            using (var writerStream = request.GetRequestStream())
            {
                writerStream.Write(byteData,0, byteData.Length);
                writerStream.Flush();
            }

            return request;
          
        }
    }
}