using RoomAid.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    public class AddUserService
    {

        /// <summary>
        /// Method AddUser will use query to insert new user and password into the database
        /// <param name="user">The user object that is already created</param>
        /// <param name="password">user's password which is hashed with salt</param>
        /// <returns>IResult result the object that contains a message and if the check is true or false</returns>
        public IResult AddUser(User newUser, string hashedPassword, string salt)
        {
            IResult addUser = null;
            IResult storePword = null;
            int retryLimit = Int32.Parse(ConfigurationManager.AppSettings["retryLimit"]);
            string message = "";
            bool ifSuccess = true;
            int retryTimes = 0;

            while (retryTimes<retryLimit)
            {
                addUser = StoreUser(newUser);
                if (!addUser.IsSuccess)
                    retryTimes++;
                    
                else
                    retryTimes = retryLimit;
            }

            retryTimes = 0;
            while (retryTimes < retryLimit)
            {
                storePword = StorePassword(newUser, hashedPassword, salt);
                if (!addUser.IsSuccess)
                    retryTimes++;
                else
                    retryTimes = retryLimit;
            }

            if (addUser.IsSuccess && storePword.IsSuccess)
            {
                message = message+ ConfigurationManager.AppSettings["success"];
            }

            if (!addUser.IsSuccess)
            {
                message = message + addUser.Message;
                ifSuccess = false;
            }

            if (!storePword.IsSuccess)
            {
                message = message + storePword.Message;
                ifSuccess = false;
            }

            return new CheckResult(ConfigurationManager.AppSettings["success"], ifSuccess);
        }

        //TODO: query to insert new user who is an admin
        public IResult AddAdmin(User newUser, string password)
        {
            return null;
        }

        /// <summary>
        /// Method FindUser will use query to find a user based on the given email
        /// <param name="email">The user's email that will be used to find certain user</param>
        /// <returns>IResult result the object that contains a message and if the check is true or false</returns>
        public IResult FindUser(string email)
        {
            IResult result = new CheckResult("This User exists!", true);
            try
            {
                //TODO: query for database to insert new user in users table and password in password table
            }
            catch (Exception e)
            {
                result = new CheckResult("Cannot find this user or an error occur: \n"+e.Message, false);
            }
            return result;
        }

        /// <summary>
        /// Method StoreUser will use query to insert a new user into the table
        /// <param name="user">The new user which is created</param>
        /// <returns>IResult result the object that contains a message and if the check is true or false</returns>
        private IResult StoreUser(User newUser)
        {
            IResult result = new CheckResult("Successfully added new user!", true);
            try
            {
                string sqlConnection = "";
                //TODO: query for database to insert new user in users table and password in password table
                using (SqlConnection connection = new SqlConnection(sqlConnection))
                {
                    String query = "INSERT INTO Users (Email,FirstName,LastName,DateOfBirth, Gender, AccountStatus) VALUES (@email,@fname,@lname, @dob,@gender, @status)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", newUser.UserEmail);
                        command.Parameters.AddWithValue("@fname", newUser.FirstName);
                        command.Parameters.AddWithValue("@lname", newUser.LastName);
                        command.Parameters.AddWithValue("@dob", newUser.DateOfBirth);
                        command.Parameters.AddWithValue("@gender", newUser.Gender);
                        command.Parameters.AddWithValue("@status", newUser.AccountStatus);

                        connection.Open();
                        int error = command.ExecuteNonQuery();

                        //query to add profile
                    }
                }
            }
            catch (Exception e)
            {
                result = new CheckResult(e.Message, false);
            }
            return result;
        }

        /// <summary>
        /// Method StorePassword will use query to insert password into the table based on the given user
        /// <param name="user">The new user which is created</param>
        /// <param name="password">The password which is requried to be stored</param>
        /// <param name="salt">The salt which is used to hash the password</param>
        /// <returns>IResult result the object that contains a message and if the check is true or false</returns>
        private IResult StorePassword(User newUser, string password, string salt)
        {
            IResult result = new CheckResult("Success!", true);
            string userEmail = newUser.UserEmail;
            try
            {
                string sqlConnection = "";
                using (SqlConnection connection = new SqlConnection(sqlConnection))
                {
                    String query = "INSERT INTO UserLogin (Email,Salt,Password) VALUES (@email,@salt,@pword)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", newUser.UserEmail);
                        command.Parameters.AddWithValue("@salt", password);
                        command.Parameters.AddWithValue("@pword", salt);

                        connection.Open();
                        int error = command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                result = new CheckResult(e.Message, false);
            }
            return result;
        }

    }
}
