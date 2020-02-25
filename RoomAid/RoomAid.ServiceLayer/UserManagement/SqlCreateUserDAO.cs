using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    public class SqlCreateUserDAO : ICreateUserDAO
    {
        private string connectionString;
        public SqlCreateUserDAO(string sqlConnection)
        {
            connectionString = sqlConnection;
        }

        public IResult Create(User newUser)
        {
            IResult result = new CheckResult("Successfully added new user!", true);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO dbo.Users (UserSystemID,FirstName,LastName,DateOfBirth, Gender, AccountStatus) VALUES (@email,@fname,@lname, @dob,@gender, @status)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", newUser.UserEmail);
                        command.Parameters.AddWithValue("@fname", newUser.FirstName);
                        command.Parameters.AddWithValue("@lname", newUser.LastName);
                        command.Parameters.AddWithValue("@dob", newUser.DateOfBirth);
                        command.Parameters.AddWithValue("@gender", newUser.Gender);
                        command.Parameters.AddWithValue("@status", newUser.AccountStatus);

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

        public int GetSystemID(User newUser)
        {
            string email = newUser.UserEmail;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["connectionStringAccount"]))
            {
                SqlCommand command = new SqlCommand("SELECT SysID FROM dbo.Users WHERE UserSystemID = @userEmail", connection);
                command.Parameters.AddWithValue("@userEmail", email);
                connection.Open();

                var UserSystemID = command.ExecuteScalar();
                if(UserSystemID != null)
                {
                    return (Int32)UserSystemID;
                }
                return -1;
            }
            
        }
    }
}
