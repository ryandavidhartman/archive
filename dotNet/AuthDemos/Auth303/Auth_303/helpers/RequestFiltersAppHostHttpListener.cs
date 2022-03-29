using System;
using System.Text;
using Funq;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Messaging;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;

namespace Auth_303.helpers
{
    public class RequestFiltersAppHostHttpListener : AppHostHttpListenerBase
    {
        private Guid _currentSessionGuid;
        public SessionContext SessionContext; 
        
        public RequestFiltersAppHostHttpListener() : base("Request Filters Tests", typeof(RequestFiltersAppHostHttpListener).Assembly) { }

        public override void Configure(Container container)
        {
            Plugins.Add(new SessionFeature());

            GlobalRequestFilters.Add((req, res, dto) =>
            {
                var userPass = req.GetBasicAuthUserAndPassword();
                if (userPass == null)
                {
                    return;
                }

                var userName = userPass.Value.Key;
                if (userName != SystemConstants.AllowedUser || userPass.Value.Value != SystemConstants.AllowedPass)
                    return;

                _currentSessionGuid = Guid.NewGuid();
                var sessionKey = userName + "/" + _currentSessionGuid.ToString("N");

                //set session for this request (as no cookies will be set on this request)
                req.Items["ss-session"] = sessionKey;
                res.SetPermanentCookie("ss-session", sessionKey);
            });

            GlobalRequestFilters.Add((req, res, dto) =>
            {
                if (!(dto is Secure))
                    return;

                var sessionId = req.GetItemOrCookie("ss-session") ?? string.Empty;
                var sessionIdParts = sessionId.SplitOnFirst('/');
                if (sessionIdParts.Length < 2 || sessionIdParts[0] != SystemConstants.AllowedUser || sessionIdParts[1] != _currentSessionGuid.ToString("N"))
                {
                    res.ReturnAuthRequired();
                    return;
                }

                ((Secure)dto).UserName = sessionIdParts[0];
            });

            GlobalMessageRequestFilters.Add((req, res,dto) =>
            {

                var tag = ((BasicRequest) req).Message.Tag;

                if (string.IsNullOrWhiteSpace(tag) || !tag.StartsWith("basic ", StringComparison.InvariantCultureIgnoreCase))
                    return;

                var creds = Encoding.UTF8.GetString(Convert.FromBase64String(tag.Substring(5)));

                var i = creds.IndexOf(':');
                var userName =  creds.Substring(0, i);
                var userPass =  creds.Substring(i+1);

               
                if (userName != SystemConstants.AllowedUser || userPass != SystemConstants.AllowedPass)
                    return;

                _currentSessionGuid = Guid.NewGuid();
                var sessionKey = userName + "/" + _currentSessionGuid.ToString("N");

                //set session for this request (as no cookies will be set on this request)
                req.Items["ss-session"] = sessionKey;
                res.SetPermanentCookie("ss-session", sessionKey);
            });

            GlobalMessageRequestFilters.Add((req, res, dto) =>
            {

                if (!(dto is Secure)) return;

                var sessionId = req.GetItemOrCookie("ss-session") ?? string.Empty;
                var sessionIdParts = sessionId.SplitOnFirst('/');
                if (sessionIdParts.Length < 2 || sessionIdParts[0] != SystemConstants.AllowedUser || sessionIdParts[1] != _currentSessionGuid.ToString("N"))
                {
                    res.ReturnAuthRequired();
                    return;
                }

                ((Secure)dto).UserName = sessionIdParts[0];
            });


            Func<IMessage, IMessage> filterSecureRequests = (message) =>
            {
                var tag = message.Tag;

                if (string.IsNullOrWhiteSpace(tag))
                    return message;

                if (tag.StartsWith("basic ", StringComparison.InvariantCultureIgnoreCase))
                {
                    var creds = Encoding.UTF8.GetString(Convert.FromBase64String(tag.Substring(5)));

                    var i = creds.IndexOf(':');
                    var userName = creds.Substring(0, i);
                    var userPass = creds.Substring(i + 1);


                    if (userName != SystemConstants.AllowedUser || userPass != SystemConstants.AllowedPass)
                    {
                        message.Tag = null;
                        return message;
                    }

                    _currentSessionGuid = Guid.NewGuid();
                    var sessionKey = userName + "/" + _currentSessionGuid.ToString("N");

                    SessionContext = new SessionContext { SessionKey = sessionKey, Username = userName };
                    container.Register(SessionContext);
                    message.Tag = sessionKey;
                    return message;
                }

                message.Tag = null;
                return message;
            };



            var sercureLogic = new SecureLogic();
            container.Register(sercureLogic);
           
            //Wire up the secure service over the message broker
            var redisFactory = new PooledRedisClientManager("localhost:6379");
            container.Register<IRedisClientsManager>(redisFactory); // req. to log exceptions in redis
            var mqHost = new RedisMqServer(redisFactory, retryCount: 2) {RequestFilter = filterSecureRequests, ResponseFilter = FilterSecureReponses};
            mqHost.RegisterHandler<Secure>(m => HandleSecureRequests(m, sercureLogic));
            mqHost.RegisterHandler<GetFactorial>(m => ServiceController.ExecuteMessage(m));
            mqHost.Start(); //Starts listening for messages
        }

        

        public object FilterSecureReponses(object res)
        {
            return res;
        }

        public SecureResponse HandleSecureRequests(IMessage<Secure> message, SecureLogic logic)
        {
            return logic.ProcessRequest(message.GetBody());
        }
    }
}
