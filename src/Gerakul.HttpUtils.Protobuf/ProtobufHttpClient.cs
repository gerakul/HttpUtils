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

        public static ProtobufHttpClient Create(string baseAddress = null,
            IEnumerable<IUntypedMessageDescriptor> descriptors = null,
            bool includeDefaultRequestPreparation = true,
            Func<HttpRequestMessage, Task> additionalRequestPreparation = null)
        {
            ProtobufContentSerializer pcs = new ProtobufContentSerializer(descriptors);

            Func<HttpRequestMessage, Task> mainRequestPreparation;
            if (includeDefaultRequestPreparation)
            {
                if (additionalRequestPreparation != null)
                {
                    mainRequestPreparation = async r =>
                    {
                        r.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/x-protobuf"));
                        await additionalRequestPreparation(r);
                    };
                }
                else
                {
                    mainRequestPreparation = r =>
                    {
                        r.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/x-protobuf"));
                        return Task.CompletedTask;
                    };
                }
            }
            else
            {
                mainRequestPreparation = additionalRequestPreparation;
            }

            return new ProtobufHttpClient(pcs, pcs, baseAddress, mainRequestPreparation);
        }
    }
}
