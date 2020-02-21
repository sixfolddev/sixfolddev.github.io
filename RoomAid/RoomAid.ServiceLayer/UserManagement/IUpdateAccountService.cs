using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer.UserManagement
{
    /// <summary>
    /// This is an interface that allows for multiple UpdateAccount services to craft
    /// different commands for different database command string creations
    /// </summary>
    public interface IUpdateAccountService
    {
        IResult Update();
    }
}
