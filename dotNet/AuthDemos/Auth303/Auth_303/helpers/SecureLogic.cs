

using ServiceStack;
using ServiceStack.Auth;

namespace Auth_303.helpers
{
    public class SecureLogic
    {
        public SecureResponse ProcessRequest(Secure request)
        {
            var context = HostContext.TryResolve<SessionContext>();
            var session = HostContext.TryResolve<IAuthSession>();
            return new SecureResponse {Result = "Confidential"};
        }

    }
}
