using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer.User_Management
{
    public class UpdateAccountDAO :IUpdateAccountDAO
    {
        private readonly String _connection;

        public UpdateAccountDAO(String connection)
        {
            this._connection = connection;
        }

        /// <summary>
        /// Executes the list of queries that is passed in with a single transaction in order to lessen performance hit
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public int Update(List<String> queries)
        {
            int rowsChanged = 0;
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();
                SqlTransaction trans = connection.BeginTransaction();
                SqlCommand cmd = new SqlCommand("",connection, trans);

                try
                {
                    foreach(String query in queries)
                    {
                        cmd.CommandText = query;
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
