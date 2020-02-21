using RoomAid.DataAccessLayer.User_Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer.UserManagement
{
    public class UpdateAccountSqlService : IUpdateAccountService
    {
        private readonly List<User> _newUsers;
        private readonly UpdateAccountDAO _update;

        /// <summary>
        /// Service that crafts queries for updating users related to a sql database
        /// </summary>
        /// <param name="newUsers"></param>
        /// <param name="update"></param>
        public UpdateAccountSqlService(List<User> newUsers, UpdateAccountDAO update)
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
            String message;
            bool isSuccess = true;
            List<String> queries = new List<String>();
            String tableName = ConfigurationManager.AppSettings["tableName"];

            //creates a set of queries within a list to pass towards a dao for updating
            foreach(User newUser in _newUsers)
            {
                queries.Add($"UPDATE {tableName} SET FirstName = {newUser.FirstName}, LastName = {newUser.LastName}," +
                    $"DateOfBirth = {newUser.DateOfBirth}, Gender = {newUser.Gender}, AccountStatus = {newUser.AccountStatus}" +
                    $" WHERE UserEmail = {newUser.UserEmail}");
            }

            //changing result based upon if all accounts were updated successfully
            int rowsChanged = _update.Update(queries);
            if(rowsChanged == queries.Count)
            {
                message = ConfigurationManager.AppSettings["successMessage"];
            }
            else
            {
                message = ConfigurationManager.AppSettings["failureMessage"];
                isSuccess = false;
            }



            return new CheckResult(message, isSuccess); 

        }
    }
}
