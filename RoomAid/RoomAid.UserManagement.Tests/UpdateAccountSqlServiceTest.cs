using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.DataAccessLayer;
using RoomAid.ServiceLayer;


namespace UserManagementTests
{
    [TestClass]
    public class UpdateAccountSqlServiceTest
    {

        //All targeted rows have been updated successfully!
        //Not all targeted rows have been updated, please review which rows were attempted at updates
        
        [TestMethod]
        public void UpdateTestSuccessful()
        {
            var result = new CheckResult("All targeted rows have been updated successfully!", true);
            var date = new DateTime(1998, 11, 13);
            var user = new User(1, "boi@gmail.com","daniel", "gione", "enabled", date, "Male");
            var dao = new UpdateAccountDAOTestSuccess(date);
            var update = new UpdateAccountSqlService(user, dao);
            
            var compare = update.Update();

            Assert.IsTrue(compare.IsSuccess);
            Assert.AreEqual(compare.Message, result.Message);

        }

        [TestMethod]
        public void UpdateTestFailure()
        {
            var result = new CheckResult("All targeted rows have been updated successfully!", true);
            var date = new DateTime(1998, 11, 13);
            var user = new User(1, "boi@gmail.com",  "daniel", "gione", "disabled", date, "Male");
            var dao = new UpdateAccountDAOTestSuccess(date);
            var update = new UpdateAccountSqlService(user, dao);

            var compare = update.Update();

            Assert.AreNotEqual(compare.IsSuccess, result.IsSuccess);
            Assert.AreNotEqual(compare.Message, result.Message);
        }

        public class UpdateAccountDAOTestSuccess: IUpdateAccountDAO
        {
            public DateTime date;
            public UpdateAccountDAOTestSuccess(DateTime date)
            {
                this.date = date;
            }

            public int Update(List<SqlCommand> commands)
            {

                var cmd = new SqlCommand();
                cmd.CommandText= "UPDATE dbo.Users SET FirstName = @fName, LastName = @lName, DateOfBirth = @dob, Gender = @gender, AccountStatus = @status WHERE UserEmail = @email";
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
