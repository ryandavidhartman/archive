using Auth_303.helpers;
using ServiceStack;

namespace Auth_303.tests
{
    public class DirectServiceClientFilterTests : RequestFiltersTests
    {
        protected override IServiceClient CreateNewServiceClient()
        {
            return new DirectServiceClient(AppHost.ServiceController);
        }

        protected override IRestClientAsync CreateNewRestClientAsync()
        {
            return null; //TODO implement REST calls with DirectServiceClient (i.e. Unit Tests)
            //EndpointHandlerBase.ServiceManager = new ServiceManager(true, typeof(SecureService).Assembly);
            //return new DirectServiceClient(EndpointHandlerBase.ServiceManager);
        }
    }
}
