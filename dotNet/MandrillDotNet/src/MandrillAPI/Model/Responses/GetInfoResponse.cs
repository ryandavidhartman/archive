using System.Runtime.Serialization;
using MandrillAPI.Model.Data;

namespace MandrillAPI.Model.Responses
{
    [DataContract(Name="user_information")]
    public class GetInfoResponse
    {
        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }

        [DataMember(Name = "public_id")]
        public string PublicId { get; set; }

        [DataMember(Name = "reputation")]
        public int Reputation { get; set; }

        [DataMember(Name = "hourly_quota")]
        public int HourlyQuota { get; set; }

        [DataMember(Name = "backlog")]
        public int Backlog { get; set; }

        [DataMember(Name = "stats")]
        public Days Stats { get; set; }
    }
}
