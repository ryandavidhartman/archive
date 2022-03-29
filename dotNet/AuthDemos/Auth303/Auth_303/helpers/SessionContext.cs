using System.Collections.Generic;


namespace Auth_303.helpers
{
    public class SessionContext
    {
        public string SessionKey { get; set; }
        public string Username { get; set; }
        public List<string> Permissions { get; set; }
        public List<string> Roles { get; set; }
    }
}
