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

        public T Result { get; private set; }
        public HttpResponseMessage Response { get; private set; }

        internal HttpResult(T result, HttpResponseMessage response, HttpClient httpClient, HttpRequestMessage httpRequestMessage)
        {
            this.Result = result;
            this.Response = response;
            this.httpClient = httpClient;
            this.httpRequestMessage = httpRequestMessage;
        }

        public void Dispose()
        {
            Response?.Dispose();
            httpRequestMessage?.Dispose();
            httpClient?.Dispose();
        }
    }
    public class HttpResult
    {
        public static HttpResult<T> Create<T>(T result, HttpResponseMessage response, HttpClient httpClient, HttpRequestMessage httpRequestMessage)
        {
            return new HttpResult<T>(result, response, httpClient, httpRequestMessage);
        }
    }
}
