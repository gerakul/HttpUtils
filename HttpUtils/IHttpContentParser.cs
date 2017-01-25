using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gerakul.HttpUtils
{
    public interface IHttpContentParser
    {
        Task<T> ParseContent<T>(HttpContent content);
    }
}
