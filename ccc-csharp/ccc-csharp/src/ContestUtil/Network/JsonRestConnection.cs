using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace CCC.ContestUtil.Network
{
    public class JsonRestConnection
    {
        public readonly NameValueCollection Header = new NameValueCollection();
        public HttpStatusCode LastStatusCode { get; private set; }
        public string ContentType = "application/json";

        private readonly string _url;
        
        /// <summary>
        /// Creates a new object which allows GET and POST
        /// to some supplied server with json content type.
        /// </summary>
        /// <param name="url">The url to communicate with</param>
        public JsonRestConnection(string url)
        {
            _url = url;
        }

        /// <summary>
        /// Creates a new JsonRestConnection and sends
        /// the supplied headers in every request.
        /// </summary>
        /// <param name="url">The url to communicate with</param>
        /// <param name="header">Headers to append every time</param>
        public JsonRestConnection(string url, NameValueCollection header) : this(url)
        {
            Header = header;
        }

        /// <summary>
        /// Creates a new JsonRestConnection and sends
        /// the supplied header name and value in every request.
        /// </summary>
        /// <param name="url">The url to communicate with</param>
        /// <param name="headerName">The name of the header</param>
        /// <param name="headerValue">The value of the header</param>
        public JsonRestConnection(string url, string headerName, string headerValue)
            : this(url, new NameValueCollection { { headerName, headerValue } }) { }

        public string Get(string path = "")
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(_url + path);
                request.Method = "GET";
                request.ContentType = ContentType;
                if (Header != null) request.Headers.Add(Header);

                var httpWebResponse = (HttpWebResponse)request.GetResponse();
                LastStatusCode = httpWebResponse.StatusCode;

                var stream = httpWebResponse.GetResponseStream();
                var response = stream == null ? null : new StreamReader(stream).ReadToEnd();
                
                Debug.WriteLine(response ?? "[null]");
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Post(string path)
        {
            return PostPut<object>("POST", path);
        }

        public string Post<T>(string path, T body = default(T))
        {
            return PostPut("POST", path, body);
        }

        public string Put(string path)
        {
            return PostPut<object>("PUT", path);
        }

        public string Put<T>(string path, T body = default(T))
        {
            return PostPut("PUT", path, body);
        }

        private string PostPut<T>(string method, string path, T body = default(T))
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(_url + path);
                request.Method = method;
                request.ContentType = ContentType;
                if (Header != null) request.Headers.Add(Header);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    if (!EqualityComparer<T>.Default.Equals(body, default(T)))
                    {
                        streamWriter.Write(JsonConvert.SerializeObject(body));
                    }

                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpWebResponse = (HttpWebResponse)request.GetResponse();
                LastStatusCode = httpWebResponse.StatusCode;

                var stream = httpWebResponse.GetResponseStream();
                var response = stream == null ? null : new StreamReader(stream).ReadToEnd();

                Debug.WriteLine(response ?? "[null]");
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
