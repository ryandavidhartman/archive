using Auth_303.helpers;
using NUnit.Framework;
using ServiceStack;

namespace Auth_303.tests
{
    [TestFixture]
    public class JsonIntegrationTests : RequestFiltersTests
    {
        protected override string GetFormat()
        {
            return "json";
        }

        protected override IServiceClient CreateNewServiceClient()
        {
            return new JsonServiceClient(SystemConstants.ServiceClientBaseUri);
        }

        protected override IRestClientAsync CreateNewRestClientAsync()
        {
            return new JsonServiceClient(SystemConstants.ServiceClientBaseUri);
        }
    }
}
