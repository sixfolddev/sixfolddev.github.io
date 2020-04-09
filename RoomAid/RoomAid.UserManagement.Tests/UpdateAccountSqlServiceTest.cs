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
        private readonly IUpdateAccountDAO _dao = new UpdateAccountSqlDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
        private ICreateAccountDAO newAccountDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User));
        private ICreateAccountDAO newMappingDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
        private IMapperDAO mapperDAO = new SqlMapperDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
        private ICreateAccountDAO newUserDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));

        [TestMethod]
        public void UpdateTestSuccessful()
        {
            DeleteDummyAccount();
            CreateDummyAccount();

            var result = new CheckResult("All targeted rows have been updated successfully!", true);
            var date = new DateTime(1998, 11, 13);
            var user = new User(1, "boi@gmail.com","daniel", "gione", "enabled", date, "Male");
            var update = new UpdateAccountSqlService(user, _dao);
            
            var compare = update.Update();

            DeleteDummyAccount();

            Assert.IsTrue(compare.IsSuccess);
            Assert.AreEqual(compare.Message, result.Message);


            
        }

        private void CreateDummyAccount()
        {
            Account testAccount = new Account("boi@gmail.com", "testHashedPassword", "testSalt");
            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, daos);
            cas.Create();
        }

        private void DeleteDummyAccount()
        {
            DeleteUser("boi@gmail.com");
            DeleteMapping("boi@gmail.com");
            DeleteAccount("boi@gmail.com");
        }

        [TestMethod]
        public void UpdateTestFailure()
        {
            DeleteDummyAccount();
            CreateDummyAccount();

            var result = new CheckResult("All targeted rows have been updated successfully!", true);
            var date = new DateTime(1998, 11, 13);
            var user = new User(1, "boi@gmail.com", "daniel", "gione", "disabled", date, "Male");
            //var dao = new UpdateAccountSqlDAO("fakeConnect");

            var update = new UpdateAccountSqlService(user,_dao );

            var compare = update.Update();

            DeleteDummyAccount();

            Assert.AreNotEqual(compare.IsSuccess, result.IsSuccess);
            Assert.AreNotEqual(compare.Message, result.Message);


        }

        //Cleanning tools
        public void DeleteMapping(string userEmail)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User)))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM dbo.Mapping Where UserEmail = @userEmail", connection);
                    command.Parameters.AddWithValue("@userEmail", userEmail);
                    using (command)
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SystemException)
            {
                throw;
            }
        }
        public void DeleteAccount(string userEmail)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User)))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM dbo.Accounts Where UserEmail = @userEmail", connection);
                    command.Parameters.AddWithValue("@userEmail", userEmail);
                    using (command)
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SystemException)
            {
                throw;
            }
        }
        //Tool method to clean testing account created by the test method
        public void DeleteUser(string userEmail)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User)))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM dbo.Users Where UserEmail = @userEmail", connection);
                    command.Parameters.AddWithValue("@userEmail", userEmail);
                    using (command)
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SystemException)
            {
                throw;
            }
        }




    }
}
