using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer
{
    public class AccountDAO
    {
        private readonly string _connection;

        public AccountDAO(string connection)
        {
            this._connection = connection;
        }
        public Dictionary<string, string> RetrieveAccountInfo(SqlCommand command)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();
                try
                {
                    using (command)
                    {
                        command.Connection = connection;
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            // Fetch data from data store
                            if (dataReader.Read())
                            {
                                Object[] accountInfo = new Object[2];
                                int count = dataReader.GetValues(accountInfo); // Count will store the number of columns retrieved
                                data.Add("Hash", accountInfo[0].ToString());
                                data.Add("Salt", accountInfo[1].ToString());
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return data;
        }
    }
}
