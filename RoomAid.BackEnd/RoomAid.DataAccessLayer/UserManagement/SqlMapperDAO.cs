using System;
using System.Configuration;
using System.Data.SqlClient;


namespace RoomAid.DataAccessLayer
{
   public class SqlMapperDAO : IMapperDAO
    {
        private readonly string _connection;
        public SqlMapperDAO(string connectionString)
        {
            this._connection = connectionString;
        }

        /// <summary>
        /// Method that return the systemID for a certain account based on input email
        /// </summary>
        ///<returns>The systemID for new account</returns>
        public int GetSysID(string input)
        {
            int sysID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connection))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["querySelectMapping"], connection);
                    command.Parameters.AddWithValue("@email", input);
                    using (command)
                    {
                        sysID = (Int32)command.ExecuteScalar();
                    }
                    connection.Close();
                }
            }
            catch (SystemException)
            {
                throw;
            }
            return sysID;
        }
    }
}
