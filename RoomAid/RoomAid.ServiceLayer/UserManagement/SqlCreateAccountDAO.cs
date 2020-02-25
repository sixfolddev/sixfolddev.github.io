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
    /// Method CreateAccount will use query to insert new user and password into the database
    /// <param name="user">The user object that is already created</param>
    /// <returns>Iresult result the object that contains a message and if the check is true or false</returns>
    /// 
    /// <summary>
    /// Method CreateUser will use query to insert a new user into the table
    /// <param name="user">The new user which is created</param>
    /// <returns>Iresult result the object that contains a message and if the check is true or false</returns>
    public class SqlCreateAccountDAO : ICreationAccountDAO
    {
        public IResult Create(User newUser)
        {
            IResult result = new CheckResult("Successfully added new user!", true);
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["sqlConnection"]))
                {
                    string query = "INSERT INTO dbo.Users (UserEmail) VALUES (@email)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", newUser.UserEmail);
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

        /// <summary>
        /// Method IfUserExist will use query to check if the email is already registered in database
        /// <param name="email">The email you want to check</param>
        /// <returns>true if exist, false if the email is not registered</returns>
        public bool IfUserExist(string email)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["sqlConnection"]))
            {
                SqlCommand command = new SqlCommand("SELECT UserEmail FROM dbo.Users WHERE UserEmail = @userEmail", connection);
                command.Parameters.AddWithValue("@userEmail", email);
                connection.Open();

                var UserEmail = command.ExecuteScalar();

                if (UserEmail != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
