using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gerakul.HttpUtils
{
    public interface IHttpContentGetter
    {
        Task<HttpContent> GetContent(object obj);
    }
}
