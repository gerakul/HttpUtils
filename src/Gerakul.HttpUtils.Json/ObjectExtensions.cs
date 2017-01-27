using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gerakul.HttpUtils.Json
{
    internal static class ObjectExtensions
    {
        public static T CastTo<T>(this object obj)
        {
            return (T)obj;
        }
        public static T CastTo<T>(this object obj, T proto)
        {
            return (T)obj;
        }
    }
}
