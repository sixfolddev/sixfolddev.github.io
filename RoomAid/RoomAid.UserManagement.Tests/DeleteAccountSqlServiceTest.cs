using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.DataAccessLayer;
using RoomAid.ServiceLayer;
using RoomAid.ServiceLayer.UserManagement;

namespace UserManagementTests
{
    [TestClass]
    public class DeleteAccountSqlServiceTest
    {

        [TestMethod]
        public void DeleteUsers_Pass()
        {
            var newUsers = new List<User>();
            var date = new DateTime(1998, 11, 13);
            var user = new User(1, "boi@gmail.com", "daniel", "gione", "enabled", date, "Male");
            newUsers.Add(user);
            var dao = new UpdateAccountDAOTestSuccess(date);
            var update = new UpdateAccountSqlService(newUsers, dao);

            IDeleteAccountDAO systemDB = new SqlDeleteAccountDAO(ConfigurationManager.AppSettings["sqlConnectionSystem"]);
            IDeleteAccountDAO mappingDB = new SqlDeleteAccountDAO(ConfigurationManager.AppSettings["sqlConnectionMapping"]);
            IDeleteAccountDAO accountDB = new SqlDeleteAccountDAO(ConfigurationManager.AppSettings["sqlConnectionAccount"]);

            IDeleteAccountService deleter = new DeleteAccountSQLService(user, systemDB, mappingDB, accountDB);
        }

        public class UpdateAccountDAOTestSuccess : IUpdateAccountDAO
        {
            public DateTime date;
            public UpdateAccountDAOTestSuccess(DateTime date)
            {
                this.date = date;
            }

            public int Update(List<SqlCommand> commands)
            {

                var cmd = new SqlCommand();
                cmd.CommandText = "UPDATE dbo.Users SET FirstName = @fName, LastName = @lName, DateOfBirth = @dob, Gender = @gender, AccountStatus = @status WHERE UserEmail = @email";
                cmd.Parameters.AddWithValue("@email", "boi@gmail.com");
                cmd.Parameters.AddWithValue("@fName", "daniel");
                cmd.Parameters.AddWithValue("@lName", "gione");
                cmd.Parameters.AddWithValue("@dob", date);
                cmd.Parameters.AddWithValue("@gender", "Male");
                cmd.Parameters.AddWithValue("@status", "enabled");

                Trace.WriteLine(cmd.CommandText);
                Trace.WriteLine(commands[0].CommandText);
                if (commands[0].CommandText.Equals(cmd.CommandText))
                    return 1;
                else
                    return 0;
            }
        }
    }
}
