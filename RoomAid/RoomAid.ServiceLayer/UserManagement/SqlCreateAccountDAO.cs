using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    /// <summary>
    /// Method CreateUser will use query to insert a new user into the table
    /// <param name="user">The new user which is created</param>
    /// <returns>Iresult result the object that contains a message and if the check is true or false</returns>
    public class SqlCreateAccountDAO : ICreateAccountDAO
    {
        private string connectionString;
        public SqlCreateAccountDAO(string sqlConnection)
        {
            connectionString = sqlConnection;
        }
        public IResult Create(User newUser)
        {
            IResult result = new CheckResult("Successfully added new Account!", true);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO dbo.Accounts (UserEmail, HashedPassword, Salt) VALUES (@email, @hashedPw, @salt)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", newUser.UserEmail);
                        command.Parameters.AddWithValue("@hashedPw", newUser.Password);
                        command.Parameters.AddWithValue("@salt", newUser.Salt);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                result = new CheckResult(e.Message, false);
            }
            return result;
        }
    }
}
