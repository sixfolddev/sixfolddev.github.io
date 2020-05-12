using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace RoomAid.ServiceLayer
{
    public class SessionTokenService
    {
        private readonly JWTService _jwt;
        public SessionTokenService()
        {

        }

        public void StartSession(User user)
        {
            string sessionToken = null;
            string seshId = "";
            string uid = "";
            Int64 iat = _jwt.getTimeNowInSeconds();
            Int64 exp = iat + Int32.Parse(ConfigurationManager.AppSettings["sessiontimeout"]);
            // Setup sessions in database to check for active session
            bool ActiveSessionExists = false; // temp placeholder
            
            
            if (!ActiveSessionExists)
            {
                sessionToken = _jwt.GenerateJWT(user);
            }

            UserSession session = new UserSession(sessionToken, seshId, iat, exp, uid, user);
           
            
        }

        // TODO: Complete method
        public string GetSessionToken(User user) // Pass in User object or masked user id?
        {
            return ""; // Change later
        }
    }
}
