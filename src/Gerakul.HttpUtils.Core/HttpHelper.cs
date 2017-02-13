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
        private static string GetValueString(PropertyInfo pi, object obj, string separator, bool escape)
        {
            var val = pi.GetValue(obj);
            var e = val as System.Collections.IEnumerable;

            if (e != null && !pi.PropertyType.Equals(typeof(string)))
            {
                return string.Join(separator, e.Cast<object>());
            }
            else
            {
                return escape ? Uri.EscapeDataString(val.ToString()) : val.ToString();
            }
        }

        public static string GetParamString(object parameters, bool escape = true)
        {
            var props = parameters.GetType().GetTypeInfo().DeclaredProperties.Where(x => x.CanRead).ToArray();

            return string.Join("&",
                props.Select(x => $"{x.Name}={GetValueString(x, parameters, ",", escape)}"));
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
    }
}
