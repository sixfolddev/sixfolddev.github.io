using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.Authorization
{
    public interface IAuthZService
    {
        bool Authorize(AuthZEnum.AuthZ[] permissions, string userID, string householdID);
    }
}
