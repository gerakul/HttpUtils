﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gerakul.HttpUtils.Core
{
    public interface ISimpleHttpClient
    {
        string BaseAddress { get; }
        Func<HttpRequestMessage, Task> MainRequestPreparation { get; }
        IHttpContentGetter ContentGetter { get; }
        IHttpContentParser ContentParser { get; }
        Task<HttpResult<T>> Send<T>(HttpMethod method, string path,
            object parameters = null, object body = null, object headers = null,
            Func<HttpRequestMessage, Task> requestPreparation = null,
            Func<object, Task<HttpContent>> customContentGetter = null,
            Func<HttpContent, Task<T>> customContentParser = null);
    }

    public static class SimpleHttpClientExtensions
    {
        public static Task<HttpResult<T>> SendAnonimous<T>(this ISimpleHttpClient obj, T proto, HttpMethod method, string path,
            object parameters = null, object body = null, object headers = null,
            Func<HttpRequestMessage, Task> requestPreparation = null,
            Func<object, Task<HttpContent>> customContentGetter = null,
            Func<HttpContent, Task<T>> customContentParser = null)
        {
            return obj.Send<T>(method, path, parameters, body, headers, requestPreparation, customContentGetter, customContentParser);
        }

        public static async Task<T> SendEnsure<T>(this ISimpleHttpClient obj, HttpMethod method, string path,
            object parameters = null, object body = null, object headers = null,
            Func<HttpRequestMessage, Task> requestPreparation = null,
            Func<object, Task<HttpContent>> customContentGetter = null,
            Func<HttpContent, Task<T>> customContentParser = null)
        {
            T resObj;
            using (var res = await obj.Send<T>(method, path, parameters, body, headers, requestPreparation, customContentGetter, customContentParser))
            {
                res.Response.EnsureSuccessStatusCode();
                resObj = res.Body;
            }

            return resObj;
        }
        public static Task<T> SendEnsureAnonimous<T>(this ISimpleHttpClient obj, T proto, HttpMethod method, string path,
            object parameters = null, object body = null, object headers = null,
            Func<HttpRequestMessage, Task> requestPreparation = null,
            Func<object, Task<HttpContent>> customContentGetter = null,
            Func<HttpContent, Task<T>> customContentParser = null)
        {
            return obj.SendEnsure<T>(method, path, parameters, body, headers, requestPreparation, customContentGetter, customContentParser);
        }
    }
}
