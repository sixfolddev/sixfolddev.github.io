using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RoomAid.DataAccessLayer
{
    public interface ISqlDAO
    {
        int RunCommand(List<SqlCommand> commands);
    }
}
