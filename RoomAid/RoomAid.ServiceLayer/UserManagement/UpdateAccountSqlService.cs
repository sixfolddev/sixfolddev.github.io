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
        private  List<User> newUsers { get; }
        private UpdateAccountDAO update { get; }

        public UpdateAccountSqlService(List<User> newUsers, UpdateAccountDAO update)
        {
            this.newUsers = newUsers;
            this.update = update;
        }

        public IResult Update()
        {
            String message;
            bool isSuccess = true;
            List<String> queries = new List<String>();
            String tableName = ConfigurationManager.AppSettings["tableName"];
            foreach(User newUser in newUsers)
            {
                queries.Add($"UPDATE {tableName} SET FirstName = {newUser.FirstName}, LastName = {newUser.LastName}," +
                    $"DateOfBirth = {newUser.DateOfBirth}, Gender = {newUser.Gender}, AccountStatus = {newUser.AccountStatus}" +
                    $" WHERE UserEmail = {newUser.UserEmail}");
            }

            int rowsChanged = update.Update(queries);
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
