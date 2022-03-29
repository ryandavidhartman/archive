using System.Runtime.Serialization;
using ServiceStack;

namespace Auth_303.helpers
{
    [Route("/factorial/{ForNumber}")]
    [DataContract]
    public class GetFactorial
    {
        [DataMember]
        public long ForNumber { get; set; }
    }

    [DataContract]
    public class GetFactorialResponse
    {
        [DataMember]
        public long Result { get; set; }
    }

    public class GetFactorialService : IService
    {
        public object Any(GetFactorial request)
        {
            return new GetFactorialResponse { Result = GetFactorial(request.ForNumber) };
        }

        public static long GetFactorial(long n)
        {
            return n > 1 ? n * GetFactorial(n - 1) : 1;
        }
    }


}
