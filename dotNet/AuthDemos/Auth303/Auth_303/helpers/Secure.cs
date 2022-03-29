using System.Runtime.Serialization;
using Amib.Threading;
using ServiceStack;
using ServiceStack.Web;

namespace Auth_303.helpers
{
    [DataContract]
    [Route("/secure")]
    public class Secure
    {
        [DataMember]
        public string UserName { get; set; }
    }

    [DataContract]
    public class SecureResponse : IHasResponseStatus
    {
        [DataMember]
        public string Result { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
    }
    
    public class SecureService : Service
    {
        public SecureLogic Logic { get; set; }

        public object Any(Secure request)
        {
            //var items = Request.Items;
            return Logic.ProcessRequest(request);
        }
    }

}
