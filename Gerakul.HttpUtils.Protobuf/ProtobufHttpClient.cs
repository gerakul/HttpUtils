using Gerakul.HttpUtils.Core;
using Gerakul.ProtoBufSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Gerakul.HttpUtils.Protobuf
{
    public class ProtobufHttpClient : CustomHttpClient
    {
        public MessageDescriptorContainer DescriptorContainer
        {
            get
            {
                return ((ProtobufContentSerializer)ContentGetter).DescriptorContainer;
            }
        }

        protected ProtobufHttpClient(IHttpContentGetter contentGetter, IHttpContentParser contentParser,
            string baseAddress = null, Func<HttpRequestMessage, Task> mainRequestPreparation = null) 
            : base(contentGetter, contentParser, baseAddress, mainRequestPreparation)
        {
        }

        public static Func<HttpRequestMessage, Task> GetMainRequestPreparation(bool includeDefaultRequestPreparation = true,
            Func<HttpRequestMessage, Task> additionalRequestPreparation = null)
        {
            if (includeDefaultRequestPreparation)
            {
                if (additionalRequestPreparation != null)
                {
                    return async r =>
                    {
                        r.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/x-protobuf"));
                        await additionalRequestPreparation(r);
                    };
                }
                else
                {
                    return r =>
                    {
                        r.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/x-protobuf"));
                        return Task.CompletedTask;
                    };
                }
            }
            else
            {
                return additionalRequestPreparation;
            }
        }

        public static ProtobufHttpClient Create(string baseAddress = null,
            IEnumerable<IUntypedMessageDescriptor> descriptors = null,
            bool includeDefaultRequestPreparation = true,
            Func<HttpRequestMessage, Task> additionalRequestPreparation = null)
        {
            ProtobufContentSerializer pcs = new ProtobufContentSerializer(descriptors);
            return new ProtobufHttpClient(pcs, pcs, baseAddress,
                GetMainRequestPreparation(includeDefaultRequestPreparation, additionalRequestPreparation));
        }

        public static ProtobufHttpClient Create(ProtobufContentSerializer protobufContentSerializer,
            string baseAddress = null,
            bool includeDefaultRequestPreparation = true,
            Func<HttpRequestMessage, Task> additionalRequestPreparation = null)
        {
            return new ProtobufHttpClient(protobufContentSerializer, protobufContentSerializer, baseAddress, 
                GetMainRequestPreparation(includeDefaultRequestPreparation, additionalRequestPreparation));
        }
    }
}
