using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;


namespace RoomAid.DataAccessLayer
{
    public class UpdateAccountSqlDAO :IUpdateAccountDAO
    {
        private readonly String _connection;

        public UpdateAccountSqlDAO(String connection)
        {
            this._connection = connection;
        }

        /// <summary>
        /// Executes the list of queries that is passed in with a single transaction in order to lessen performance hit
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public int Update(List<SqlCommand> commands)
        {
            int rowsChanged = 0;
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();
                SqlTransaction trans = connection.BeginTransaction();
                try
                {
                    foreach(SqlCommand cmd in commands)
                    {
                        cmd.Connection =connection;
                        cmd.Transaction = trans;
                        rowsChanged += cmd.ExecuteNonQuery();
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
