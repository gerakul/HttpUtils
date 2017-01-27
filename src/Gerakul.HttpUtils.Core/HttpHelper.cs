using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;

namespace Gerakul.HttpUtils.Core
{
    internal static class HttpHelper
    {
        public static string GetParamString(object parameters, bool escape = true)
        {
            var props = parameters.GetType().GetTypeInfo().DeclaredProperties.Where(x => x.CanRead).ToArray();

            return string.Join("&",
                props.Select(x => $"{x.Name}={(escape ? Uri.EscapeDataString(x.GetValue(parameters).ToString()) : x.GetValue(parameters))}"));
        }

        public static string AddParams(string uri, object parameters, bool escape = true)
        {
            var ps = GetParamString(parameters);
            if (!string.IsNullOrWhiteSpace(ps))
            {
                return $"{uri}?{ps}";
            }
            return uri;
        }

        public static void AddHeaders(HttpRequestMessage mess, object headers)
        {
            var props = headers.GetType().GetTypeInfo().DeclaredProperties.Where(x => x.CanRead).ToArray();

            foreach (var p in props)
            {
                mess.Headers.Add(p.Name, p.GetValue(headers).ToString());
            }
        }
    }
}
