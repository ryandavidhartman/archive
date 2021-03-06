using System;
using System.IO;
using ServiceStack;
using ServiceStack.Web;

namespace Auth_101.WebAPI.Clients
{
    public class HtmlServiceClient : ServiceClientBase
    {
        public HtmlServiceClient()
        {
        }

        public HtmlServiceClient(string baseUri)
            // Can't call SetBaseUri as that appends the format specific suffixes.
            : base(baseUri, baseUri)
        {
        }

        public override string Format
        {
            // Don't return a format as we are not using a ServiceStack format specific endpoint, but 
            // rather the general purpose endpoint (just like a html <form> POST would use).
            get { return null; }
        }

        public override string Accept
        {
            get { return MimeTypes.Html; }
        }

        public override string ContentType
        {
            // Only used by the base class when POST-ing.
            get { return MimeTypes.FormUrlEncoded; }
        }

        public override void SerializeToStream(IRequest requestContext, object request, Stream stream)
        {
            var queryString = QueryStringSerializer.SerializeToString(request);
            stream.Write(queryString);
        }

        public override T DeserializeFromStream<T>(Stream stream)
        {
            return (T) DeserializeDtoFromHtml(typeof (T), stream);
        }

        public override StreamDeserializerDelegate StreamDeserializer
        {
            get { return DeserializeDtoFromHtml; }
        }

        private object DeserializeDtoFromHtml(Type type, Stream fromStream)
        {
            // TODO: No tests currently use the response, but this could be something that will come in handy later.
            // It isn't trivial though, will have to parse the HTML content.
            return Activator.CreateInstance(type);
        }
    }
}