using System.Runtime.Serialization;
using ServiceStack;

namespace Auth_303.helpers
{
    [DataContract]
    [Route("/insecure")]
    public class Insecure
    {
        [DataMember]
        public string UserName { get; set; }
    }

    [DataContract]
    public class InsecureResponse : IHasResponseStatus
    {
        [DataMember]
        public string Result { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class InsecureService : IService
    {
        public object Any(Insecure request)
        {
            return new InsecureResponse { Result = "Public" };
        }
    }
}
