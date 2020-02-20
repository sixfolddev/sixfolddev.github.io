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
            if (permissions.Length == 0 && userID.Equals(defaultUserID) && householdID.Equals(defaultHouseholdID))
            {
                return true;
            }

            else
            {
                if (!_authZ.UserID.Equals(userID))
                {
                    return false;
                }

                if (!_authZ.HouseholdID.Equals(householdID))
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
