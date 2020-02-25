using System;
using System.Configuration;
using System.Data.SqlClient;


namespace RoomAid.ServiceLayer
{
    /// <summary>
    /// Method CreateUser will use query to insert a new user into the table
    /// <param name="user">The new user which is created</param>
    /// <returns>Iresult result the object that contains a message and if the check is true or false</returns>
    public class SqlCreateAccountDAO : ICreateAccountDAO
    {
        private User newUser;
        private int systemID;
        public SqlCreateAccountDAO(User newUser)
        {
            this.newUser = newUser;
        }
        public IResult CreateAccount(string sqlConnection)
        {
            IResult result = new CheckResult("Successfully added new Account!", true);
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnection))
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

        public IResult CreateUser(string sqlConnection)
        {
            IResult result = new CheckResult("Successfully added new user!", true);
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnection))
                {
                    string query = "INSERT INTO dbo.Users (FirstName,LastName,DateOfBirth, Gender, AccountStatus) VALUES (@email,@fname,@lname, @dob,@gender, @status)";

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

                    query = "SELECT SysID FROM dbo.Users WHERE UserSystemID = @userEmail";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userEmail", newUser.UserEmail);
                        connection.Open();

                        systemID = (Int32)command.ExecuteScalar();
                        SystemID = systemID;
                    }
                }
            }
            catch (SqlException e)
            {
                result = new CheckResult(e.Message, false);
            }
            return result;
        }

        public IResult CreateMapping(string sqlConnection)
        {
            IResult result = new CheckResult("Successfully mapped the userEmail and systemID!", true);
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnection))
                {
                    string query = "INSERT INTO dbo.Mapping (SysID,UserEmail) VALUES (@systemID,@email)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fname", systemID);
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

        public int SystemID { get; set; }
    }
}
