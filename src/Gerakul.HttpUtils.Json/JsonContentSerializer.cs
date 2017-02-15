using Gerakul.HttpUtils.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gerakul.HttpUtils.Json
{
    public class JsonContentSerializer : IHttpContentGetter, IHttpContentParser
    {
        private JsonSerializerSettings jsonSerializerSettings;

        public JsonContentSerializer(JsonSerializerSettings jsonSerializerSettings = null)
        {
            this.jsonSerializerSettings = jsonSerializerSettings;
        }

        public Task<HttpContent> GetContent(object obj)
        {
            var body = obj is string ? (string)obj : JsonConvert.SerializeObject(obj, jsonSerializerSettings);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            return Task.FromResult((HttpContent)content);
        }

        public async Task<T> ParseContent<T>(HttpContent content)
        {
            var respBody = await content.ReadAsStringAsync();
            T obj = typeof(T).Equals(typeof(string)) ? respBody.CastTo<T>() : JsonConvert.DeserializeObject<T>(respBody, jsonSerializerSettings);
            return obj;
        }
    }
}
