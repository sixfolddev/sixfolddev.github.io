using RoomAid.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace RoomAid.ServiceLayer
{
    public class SqlCreateUserService : ICreateUserService
    {
        private readonly User _newUser;
        private readonly IUpdateAccountDAO _update;

        /// <summary>
        /// Service that crafts queries for updating a single user related to a sql database
        /// </summary>
        /// <param name="newUsers"></param>
        /// <param name="update"></param>
        public SqlCreateUserService(User newUser, IUpdateAccountDAO update)
        {
            this._newUser = newUser;
            this._update = update;
        }
        /// <summary>
        /// this is the publicly accessed interface method that returns an IResult based on success of updating all accounts
        /// </summary>
        /// <returns>CheckResult</returns>
        public IResult CreateUser()
        {
            String message = "";
            bool isSuccess = true;
            List<SqlCommand> commands = new List<SqlCommand>();
            var cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateUser"]);
            cmd.Parameters.AddWithValue("@email", _newUser.UserEmail);
            cmd.Parameters.AddWithValue("@fName", _newUser.FirstName);
            cmd.Parameters.AddWithValue("@lName", _newUser.LastName);
            cmd.Parameters.AddWithValue("@dob", _newUser.DateOfBirth);
            cmd.Parameters.AddWithValue("@gender", _newUser.Gender);
            cmd.Parameters.AddWithValue("@status", _newUser.AccountStatus);
            commands.Add(cmd);


            //changing result based upon if all accounts were updated successfully
            int rowsChanged = _update.Update(commands);
            if (rowsChanged == commands.Count)
            {
                message += ConfigurationManager.AppSettings["successMessage"];
            }
            else
            {
                message += ConfigurationManager.AppSettings["failureMessage"];
                isSuccess = false;
            }
            return new CheckResult(message, isSuccess);

        }
    }
}
