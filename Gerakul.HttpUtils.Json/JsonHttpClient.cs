using Gerakul.HttpUtils.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Gerakul.HttpUtils.Json
{
    public class JsonHttpClient : CustomHttpClient
    {
        protected JsonHttpClient(IHttpContentGetter contentGetter, IHttpContentParser contentParser, 
            string baseAddress = null, Func<HttpRequestMessage, Task> mainRequestPreparation = null) 
            : base(contentGetter, contentParser, baseAddress, mainRequestPreparation)
        {
        }

        public static JsonHttpClient Create(string baseAddress = null, Func<HttpRequestMessage, Task> mainRequestPreparation = null,
            JsonSerializerSettings jsonSerializerSettings = null)
        {
            JsonContentSerializer jcs = new JsonContentSerializer(jsonSerializerSettings);
            return new JsonHttpClient(jcs, jcs, baseAddress, mainRequestPreparation);
        }

        public static JsonHttpClient Create(JsonContentSerializer jsonContentSerializer, 
            string baseAddress = null, Func<HttpRequestMessage, Task> mainRequestPreparation = null)
        {
            return new JsonHttpClient(jsonContentSerializer, jsonContentSerializer, baseAddress, mainRequestPreparation);
        }
    }
}
