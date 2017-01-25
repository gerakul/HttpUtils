using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gerakul.HttpUtils
{
    public class JsonContentSerializer : IHttpContentGetter, IHttpContentParser
    {
        public Task<HttpContent> GetContent(object obj)
        {
            var body = obj is string ? (string)obj : Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            return Task.FromResult((HttpContent)content);
        }

        public async Task<T> ParseContent<T>(HttpContent content)
        {
            var respBody = await content.ReadAsStringAsync();

            T obj = typeof(T).Equals(typeof(string)) ? respBody.CastTo<T>() : Newtonsoft.Json.JsonConvert.DeserializeObject<T>(respBody);
            return obj;
        }
    }
}
