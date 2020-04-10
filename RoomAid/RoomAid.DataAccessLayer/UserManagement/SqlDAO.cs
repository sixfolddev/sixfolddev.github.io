using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer
{
    public class SqlDAO : ISqlDAO
    {
        private readonly String _connection;
        public SqlDAO(String connection)
        {
            this._connection = connection;
        }
        /// <summary>
        /// Execution of the list of queries provided by the service
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public int RunCommand(List<SqlCommand> commands)
        {
            int rowsDeleted = 0;
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();
                SqlTransaction trans = connection.BeginTransaction();
                try
                {
                    foreach(SqlCommand cmd in commands)
                    {
                        cmd.Connection = connection;
                        cmd.Transaction = trans;
                        rowsDeleted += cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch(Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
            return rowsDeleted;
        }
    }
}
