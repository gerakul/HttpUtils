using Gerakul.HttpUtils.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace Gerakul.HttpUtils.Json
{
    public class JsonHttpClient : CustomHttpClient
    {
        protected JsonHttpClient(IHttpContentGetter contentGetter, IHttpContentParser contentParser, 
            string baseAddress = null, Func<HttpRequestMessage, Task> mainRequestPreparation = null) 
            : base(contentGetter, contentParser, baseAddress, mainRequestPreparation)
        {
        }

        public static JsonHttpClient Create(string baseAddress = null, Func<HttpRequestMessage, Task> mainRequestPreparation = null)
        {
            JsonContentSerializer jcs = new JsonContentSerializer();
            return new JsonHttpClient(jcs, jcs, baseAddress, mainRequestPreparation);
        }
    }
}
