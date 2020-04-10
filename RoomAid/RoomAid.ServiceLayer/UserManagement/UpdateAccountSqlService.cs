using RoomAid.DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    public class UpdateAccountSqlService : IUpdateAccountService
    {
        private readonly List<User> _newUsers;
        private readonly ISqlDAO _update;

        /// <summary>
        /// Service that crafts queries for updating a single user related to a sql database
        /// </summary>
        /// <param name="newUsers"></param>
        /// <param name="update"></param>
        public UpdateAccountSqlService(User newUser, ISqlDAO update)
        {
            this._newUsers = new List<User>();
            this._newUsers.Add(newUser);
            this._update = update;
        }

        /// <summary>
        /// Service that crafts queries for updating users related to a sql database
        /// </summary>
        /// <param name="newUsers"></param>
        /// <param name="update"></param>
        public UpdateAccountSqlService(List<User> newUsers, ISqlDAO update)
        {
            this._newUsers = newUsers;
            this._update = update;
        }

        /// <summary>
        /// this is the publicly accessed interface method that returns an IResult based on success of updating all accounts
        /// </summary>
        /// <returns>CheckResult</returns>
        public IResult Update()
        {
            String message = "";
            bool isSuccess = true;
            List<SqlCommand> commands = new List<SqlCommand>();

            //creates a set of queries within a list to pass towards a dao for updating
            foreach(User newUser in _newUsers)
            {
                var cmd = new SqlCommand(ConfigurationManager.AppSettings["updateCmd"]);
                cmd.Parameters.AddWithValue("@email", newUser.UserEmail);
                cmd.Parameters.AddWithValue("@fName", newUser.FirstName);
                cmd.Parameters.AddWithValue("@lName", newUser.LastName);
                cmd.Parameters.AddWithValue("@dob", newUser.DateOfBirth);
                cmd.Parameters.AddWithValue("@gender", newUser.Gender);
                cmd.Parameters.AddWithValue("@status", newUser.AccountStatus);

                commands.Add(cmd);

            }

            //changing result based upon if all accounts were updated successfully
            int rowsChanged = _update.RunCommand(commands);
            if(rowsChanged == commands.Count)
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
