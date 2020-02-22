using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            var newUsers = new List<User>();
            var date = new DateTime(1998, 11, 13);
            var user = new User("boi@gmail.com", "daniel", "gione", "enabled", date, "Male");
            newUsers.Add(user);
            var dao = new UpdateAccountDAOTestSuccess(date);
            var update = new UpdateAccountSqlService(newUsers, dao);
            
            var compare = update.Update();
            Console.WriteLine(compare.Message);
            Console.WriteLine(compare.IsSuccess);
            Assert.AreEqual(result, compare);

        }

        [TestMethod]
        public void UpdatestFailure()
        {
            var result = new CheckResult("All targeted rows have been updated successfully!", true);
            var newUsers = new List<User>();
            var date = new DateTime(1998, 11, 13);
            var user = new User("boi@gmail.com", "daniel", "gione", "disabled", date, "Male");
            newUsers.Add(user);
            var dao = new UpdateAccountDAOTestSuccess(date);
            var update = new UpdateAccountSqlService(newUsers, dao);

            var compare = update.Update();
            Console.WriteLine(compare.Message);
            Console.WriteLine(compare.IsSuccess);
            Assert.AreNotEqual(result, compare);
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
                
                if(true)

                    return 1;
                else
                    return 0;
            }
        }

        


    }
}
