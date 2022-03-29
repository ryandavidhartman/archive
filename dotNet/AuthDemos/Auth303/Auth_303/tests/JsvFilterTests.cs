using Auth_303.helpers;
using NUnit.Framework;
using ServiceStack;

namespace Auth_303.tests
{
    [TestFixture]
    public class JsvIntegrationTests : RequestFiltersTests
    {
        protected override string GetFormat()
        {
            return "jsv";
        }

        protected override IServiceClient CreateNewServiceClient()
        {
            return new JsvServiceClient(SystemConstants.ServiceClientBaseUri);
        }

        protected override IRestClientAsync CreateNewRestClientAsync()
        {
            return new JsvServiceClient(SystemConstants.ServiceClientBaseUri);
        }
    }
}
