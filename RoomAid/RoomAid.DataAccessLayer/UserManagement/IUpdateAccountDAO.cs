using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RoomAid.DataAccessLayer
{
    public interface IUpdateAccountDAO
    {
        int Update(List<SqlCommand> queries);
    }
}
