using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gerakul.HttpUtils.Core
{
    public interface IHttpContentGetter
    {
        Task<HttpContent> GetContent(object obj);
    }
}
