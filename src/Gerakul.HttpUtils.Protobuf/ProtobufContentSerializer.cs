using Gerakul.HttpUtils.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Gerakul.ProtoBufSerializer;
using System.Reflection;


namespace Gerakul.HttpUtils.Protobuf
{
    public class ProtobufContentSerializer : IHttpContentGetter, IHttpContentParser
    {
        private static readonly Type IEnumerableTypeDef = typeof(IEnumerable<int>).GetGenericTypeDefinition();

        public MessageDescriptorContainer DescriptorContainer { get; private set; } = new MessageDescriptorContainer();


        public ProtobufContentSerializer(IEnumerable<IUntypedMessageDescriptor> descriptors = null)
        {
            if (descriptors != null)
            {
                DescriptorContainer.AddRange(descriptors);
            }
        }

        public Task<HttpContent> GetContent(object obj)
        {
            byte[] body = null;

            if (obj is byte[])
            {
                body = (byte[])obj;
            }
            else
            {
                var descriptor = DescriptorContainer.GetUntyped(obj.GetType());
                if (descriptor != null)
                {
                    body = descriptor.Write(obj);
                }
                else
                {
                    var ienum = obj.GetType().GetTypeInfo().ImplementedInterfaces.Where(x => x.IsConstructedGenericType)
                        .FirstOrDefault(x => x.GetGenericTypeDefinition().Equals(IEnumerableTypeDef));

                    if (ienum != null)
                    {
                        descriptor = DescriptorContainer.GetUntyped(ienum.GenericTypeArguments.First());

                        if (descriptor != null)
                        {
                            body = descriptor.WriteLenDelimitedStream((System.Collections.IEnumerable)obj);
                        }
                        else
                        {
                            throw new InvalidOperationException($"MessageDescriptor not found for {ienum.GenericTypeArguments.First()}");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException($"MessageDescriptor not found for {obj.GetType()}");
                    }
                }
            }

            var content = new ByteArrayContent(body);
            return Task.FromResult((HttpContent)content);
        }

        public async Task<T> ParseContent<T>(HttpContent content)
        {
            var respBody = await content.ReadAsByteArrayAsync();

            T obj;
            if (typeof(T).Equals(typeof(byte[])))
            {
                obj = respBody.CastTo<T>();
            }
            else
            {
                var descriptor = DescriptorContainer.GetUntyped<T>();
                if (descriptor != null)
                {
                    obj = (T)descriptor.Read(respBody);
                }
                else
                {
                    if (typeof(T).IsArray)
                    {
                        descriptor = DescriptorContainer.GetUntyped(typeof(T).GetElementType());
                        if (descriptor != null)
                        {
                            var objArr = descriptor.ReadLenDelimitedStream(respBody).Cast<object>().ToArray();
                            var arr = Array.CreateInstance(typeof(T).GetElementType(), objArr.Length);
                            Array.Copy(objArr, arr, objArr.Length);
                            obj = arr.CastTo<T>();
                        }
                        else
                        {
                            throw new InvalidOperationException($"MessageDescriptor not found for {typeof(T).GetElementType()}");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException($"MessageDescriptor not found for {typeof(T)}");
                    }
                }
            }

            return obj;
        }
    }
}
