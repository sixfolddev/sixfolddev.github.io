using System;
using System.Collections.Generic;

namespace RoomAid.ServiceLayer
{
    public class UserSession
    {

        // Public accessors
        public string Token { get; set; }
        public string SessionId { get; set; }
        public Int64 IssueTime { get; set; }
        public Int64 ExpirationTime { get; set; }
        public string UserId { get; set; }
        public User UserCurrentSession { get; set; }

  
    public UserSession(string token, string sid, Int64 iat, Int64 exp, string uid, User user)
        {
            Token = token;
            SessionId = sid;
            IssueTime = iat;
            ExpirationTime = exp;
            UserId = uid;
            UserCurrentSession = user;
        }
    }
}
