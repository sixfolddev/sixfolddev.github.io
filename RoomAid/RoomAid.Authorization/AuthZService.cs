using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.Authorization
{
    public class AuthZService: IAuthZService
    {
        private AuthZClaims _authZ;
        private const string defaultUserID = "";
        private const string defaultHouseholdID = "";

        public AuthZService(AuthZClaims claims)
        {
            _authZ = claims;
        }
        
        
        public bool Authorize(AuthZEnum.AuthZ[] permissions, string userID = defaultUserID, string householdID=defaultHouseholdID)
        {
            if (permissions.Length.Equals(0) && userID.Equals(defaultUserID) && householdID.Equals(defaultHouseholdID))
            {
                return true;
            }

            else
            {
                if (!userID.Equals(defaultUserID) && !_authZ.UserID.Equals(userID))
                {
                    return false;
                }

                if (!householdID.Equals(defaultHouseholdID) && !_authZ.HouseholdID.Equals(householdID))
                {
                    return false;
                }

                foreach (AuthZEnum.AuthZ permission in permissions)
                {
                    if (!_authZ.Claims.Contains(permission))
                    {
                        return false;
                    }
                }
            }

            return true;
            
        }
        
    }
}
