using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gerakul.HttpUtils.Core
{
    public class HttpResult<T> : IDisposable
    {
        private HttpClient httpClient;
        private HttpRequestMessage httpRequestMessage;

        public T Body { get; private set; }
        public HttpResponseMessage Response { get; private set; }

        internal HttpResult(T body, HttpResponseMessage response, HttpClient httpClient, HttpRequestMessage httpRequestMessage)
        {
            this.Body = body;
            this.Response = response;
            this.httpClient = httpClient;
            this.httpRequestMessage = httpRequestMessage;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Response?.Dispose();
                httpRequestMessage?.Dispose();
                httpClient?.Dispose();
            }
        }
    }
    public class HttpResult
    {
        public static HttpResult<T> Create<T>(T body, HttpResponseMessage response, HttpClient httpClient, HttpRequestMessage httpRequestMessage)
        {
            return new HttpResult<T>(body, response, httpClient, httpRequestMessage);
        }
    }
}
