using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer
{
    public class SqlCreateAccountDAO : ICreateAccountDAO
    {
        private readonly string _connection;

        public SqlCreateAccountDAO(string connection)
        {
            this._connection = connection;
        }

        /// <summary>
        /// Executes the list of queries that is passed in with a single transaction in order to lessen performance hit
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public int RunQuery(SqlCommand command)
        {
            int rowsChanged = 0;
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();
                SqlTransaction trans = connection.BeginTransaction();
                try
                {
                    using (command)
                    {
                        command.Connection = connection;
                        command.Transaction = trans;
                        rowsChanged =command.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
            return rowsChanged;
        }
    }
}

