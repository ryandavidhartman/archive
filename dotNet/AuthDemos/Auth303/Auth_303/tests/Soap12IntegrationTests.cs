using Auth_303.helpers;
using NUnit.Framework;
using ServiceStack;

namespace Auth_303.tests
{
    [TestFixture]
    public class Soap12IntegrationTests : RequestFiltersTests
    {
        protected override IServiceClient CreateNewServiceClient()
        {
            return new Soap12ServiceClient(SystemConstants.ServiceClientBaseUri);
        }

        protected override IRestClientAsync CreateNewRestClientAsync()
        {
            return null;
        }
    }
}
