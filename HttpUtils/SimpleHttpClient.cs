using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gerakul.HttpUtils
{
    public class SimpleHttpClient : ISimpleHttpClient
    {
        public SimpleHttpClient(IHttpContentGetter contentGetter, IHttpContentParser contentParser,
            string baseAddress = null, Func<HttpRequestMessage, Task> mainRequestPreparation = null)
        {
            this.ContentGetter = contentGetter;
            this.ContentParser = contentParser;
            this.BaseAddress = baseAddress;
            this.MainRequestPreparation = mainRequestPreparation;
        }

        public string BaseAddress { get; private set; }
        public Func<HttpRequestMessage, Task> MainRequestPreparation { get; private set; }
        public IHttpContentGetter ContentGetter { get; private set; }
        public IHttpContentParser ContentParser { get; private set; }

        public async Task<HttpResult<T>> Send<T>(HttpMethod method, string path,
            object parameters = null, object body = null, object headers = null,
            Func<HttpRequestMessage, Task> requestPreparation = null)
        {
            HttpClient httpClient = new HttpClient();
            string uri = string.IsNullOrWhiteSpace(BaseAddress) ? path : $"{BaseAddress}/{path}";

            if (parameters != null)
            {
                uri = HttpHelper.AddParams(uri, parameters);
            }

            HttpRequestMessage mess = new HttpRequestMessage(method, uri);

            if (body != null)
            {
                mess.Content = await ContentGetter.GetContent(body);
            }

            if (headers != null)
            {
                HttpHelper.AddHeaders(mess, headers);
            }

            if (MainRequestPreparation != null)
            {
                await MainRequestPreparation(mess);
            }

            if (requestPreparation != null)
            {
                await requestPreparation(mess);
            }

            var resp = await httpClient.SendAsync(mess);

            var obj = await ContentParser.ParseContent<T>(resp.Content);
            return HttpResult.Create(obj, resp, httpClient, mess);
        }
    }
}
