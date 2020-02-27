using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RoomAid.DataAccessLayer
{
    public interface IDeleteAccountDAO
    {
        int Delete(List<SqlCommand> queries);
    }
}
