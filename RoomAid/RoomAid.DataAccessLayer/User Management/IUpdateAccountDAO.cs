using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer.User_Management
{
    public interface IUpdateAccountDAO
    {
        int Update(List<String> queries);
    }
}
