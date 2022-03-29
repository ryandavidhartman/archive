using Auth_303.helpers;
using ServiceStack;

namespace Auth_303.tests
{
    public class XmlIntegrationTests : RequestFiltersTests
    {
        protected override string GetFormat()
        {
            return "xml";
        }

        protected override IServiceClient CreateNewServiceClient()
        {
            return new XmlServiceClient(SystemConstants.ServiceClientBaseUri);
        }

        protected override IRestClientAsync CreateNewRestClientAsync()
        {
            return new XmlServiceClient(SystemConstants.ServiceClientBaseUri);
        }
    }
}
