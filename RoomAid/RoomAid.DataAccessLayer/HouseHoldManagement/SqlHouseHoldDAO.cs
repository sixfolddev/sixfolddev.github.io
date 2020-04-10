using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer
{
    public class SqlHouseHoldDAO : IHouseHoldDAO
    {
        private readonly string _connection;

        public SqlHouseHoldDAO (string connection)
        {
            this._connection = connection;
        }
        public int Insert(SqlCommand command)
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
                        rowsChanged = command.ExecuteNonQuery();
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


        public int Retrieve(SqlCommand command)
        {
            int result = 0;
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
                        result = (int)command.ExecuteScalar();
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
            return result;
        }
    }
}
