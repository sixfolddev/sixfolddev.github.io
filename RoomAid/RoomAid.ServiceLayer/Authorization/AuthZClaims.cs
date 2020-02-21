using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.Authorization
{
    public class AuthZClaims
    {
        public AuthZClaims()
        {
            UserID = "";
            HouseholdID = "";
            Claims = new AuthZEnum.AuthZ[] { };
        }
       public AuthZClaims(string id, string householdID, AuthZEnum.AuthZ[] claims)
        {
            UserID = id;
            HouseholdID = householdID;
            Claims = claims;
        }
        public string UserID { get; }
        public string HouseholdID { get; }
        public AuthZEnum.AuthZ[] Claims { get; }
    }
}
