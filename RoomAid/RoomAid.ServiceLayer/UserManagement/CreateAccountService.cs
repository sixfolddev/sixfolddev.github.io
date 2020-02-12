using RoomAid.ServiceLayer.Registration;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace RoomAid.ServiceLayer.UserManagement
{
    public class CreateAccountService
    {

        /// <summary>
        /// Method CreateAccount will use query to insert new user and password into the database
        /// <param name="user">The user object that is already created</param>
        /// <returns>Iresult result the object that contains a message and if the check is true or false</returns>
        public IResult CreateAccount(User newUser)
        {
            IResult addUser = null;
            int retryLimit = Int32.Parse(ConfigurationManager.AppSettings["retryLimit"]);
            string message = "";
            bool ifSuccess = true;
            int retryTimes = 0;

            ValidationService vs = new ValidationService();
            if (!IfUserExist(newUser.UserEmail))
            {
                while (retryTimes < retryLimit)
                {
                    addUser = CreateUser(newUser);
                    if (!addUser.IsSuccess)
                        retryTimes++;

                    else
                        retryTimes = retryLimit;
                }
            }
            else
            {
                message = message + ConfigurationManager.AppSettings["userExist"];
                ifSuccess = false;
                return new CheckResult(message, ifSuccess);
            }


            if (addUser.IsSuccess)
            {
                message = message + ConfigurationManager.AppSettings["success"];
            }

            if (!addUser.IsSuccess)
            {
                message = message + addUser.Message;
                ifSuccess = false;
            }

            return new CheckResult(message, ifSuccess);
        }

        /// <summary>
        /// Method CreateUser will use query to insert a new user into the table
        /// <param name="user">The new user which is created</param>
        /// <returns>Iresult result the object that contains a message and if the check is true or false</returns>
        private IResult CreateUser(User newUser)
        {
            IResult result = new CheckResult("Successfully added new user!", true);
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["sqlConnection"]))
                {
                    string query = "INSERT INTO dbo.Users (UserEmail,FirstName,LastName,DateOfBirth, Gender, AccountStatus) VALUES (@email,@fname,@lname, @dob,@gender, @status)";

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