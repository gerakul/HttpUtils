using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gerakul.HttpUtils.Core
{
    public class CustomHttpClient : ISimpleHttpClient
    {
        protected CustomHttpClient(IHttpContentGetter contentGetter, IHttpContentParser contentParser,
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
            object parameters = null, object body = null,
            Func<HttpRequestMessage, Task> requestPreparation = null,
            Func<object, Task<HttpContent>> customContentGetter = null,
            Func<HttpContent, Task<T>> customContentParser = null)
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
                mess.Content = customContentGetter != null ? 
                    await customContentGetter(body) : await ContentGetter.GetContent(body);
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

            var obj = customContentParser != null ? await customContentParser(resp.Content) : await ContentParser.ParseContent<T>(resp.Content);
            return HttpResult.Create(obj, resp, httpClient, mess);
        }

        public static CustomHttpClient Create(IHttpContentGetter contentGetter, IHttpContentParser contentParser,
            string baseAddress = null, Func<HttpRequestMessage, Task> mainRequestPreparation = null)
        {
            return new CustomHttpClient(contentGetter, contentParser, baseAddress, mainRequestPreparation);
        }
    }
}
