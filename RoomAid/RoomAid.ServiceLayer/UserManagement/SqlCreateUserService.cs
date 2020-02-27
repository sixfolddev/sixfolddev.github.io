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
        private readonly IUpdateAccountDAO _sqlDAO;

        /// <summary>
        /// Service that crafts queries for inserting a new row in the user table 
        /// </summary>
        /// <param name="newUsers"></param>
        /// <param name="sqlDAO"></param>
        public SqlCreateUserService(User newUser, IUpdateAccountDAO sqlDAO)
        {
            this._newUser = newUser;
            this._sqlDAO = sqlDAO;
        }
        /// <summary>
        /// this is the publicly accessed interface method that returns an IResult based on success of excute the query
        /// to insert a new row in the user table
        /// </summary>
        /// <returns>IResult true or false with error message</returns>
        public IResult CreateUser()
        {
            String message = "";
            bool isSuccess = true;
            List<SqlCommand> commands = new List<SqlCommand>();
            var cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateUser"]);
            cmd.Parameters.AddWithValue("@sysId", _newUser.SystemID);
            cmd.Parameters.AddWithValue("@email", _newUser.UserEmail);
            cmd.Parameters.AddWithValue("@fName", _newUser.FirstName);
            cmd.Parameters.AddWithValue("@lName", _newUser.LastName);
            cmd.Parameters.AddWithValue("@dob", _newUser.DateOfBirth);
            cmd.Parameters.AddWithValue("@gender", _newUser.Gender);
            cmd.Parameters.AddWithValue("@status", _newUser.AccountStatus);
            commands.Add(cmd);


            //changing result based upon if all accounts were updated successfully
            int rowsChanged = _sqlDAO.Update(commands);
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
