using System;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.DataAccessLayer;
using RoomAid.ServiceLayer;

namespace RoomAid.CreateAccount.Tests
{
    [TestClass]
    public class CreateAccountTests
    {
        private IUpdateAccountDAO createAccountDAO=  new UpdateAccountSqlDAO(ConfigurationManager.AppSettings["sqlConnectionAccount"]);
        private IUpdateAccountDAO createMappingDAO = new UpdateAccountSqlDAO(ConfigurationManager.AppSettings["sqlConnectionMapping"]);
        private IUpdateAccountDAO createUserDAO = new UpdateAccountSqlDAO(ConfigurationManager.AppSettings["sqlConnectionSystem"]);

        //Test to check if the CreateAccoutnService can successfully connect to the database and create a user account
        [TestMethod]
        public void CreateAccountPass()
        {
            //Arrange
            bool expected = true;
            //Act
            Account testAccount = new Account("testerEmail","testHashedPassword", "testSalt");
            DeleteAccount(testAccount.UserEmail);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, createAccountDAO);
            IResult result = cas.Create();
            bool actual = result.IsSuccess;
            DeleteAccount(testAccount.UserEmail);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Test that if an email is already registered, the system should throw an exception
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void CreateAccountNotPass()
        {
            //Arrange
            bool expected = false;

            //Act
            Account testAccount = new Account("testerEmail", "testHashedPassword", "testSalt");
            DeleteAccount(testAccount.UserEmail);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, createAccountDAO);
            cas.Create();
            IResult result = cas.Create();
            bool actual = result.IsSuccess;
            DeleteAccount(testAccount.UserEmail);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Test to check if the CreateAccoutnService can successfully connect to the database and create mapping row
        //Should return a userID which is >=0;
        [TestMethod]
        public void CreateMappingPass()
        {
            //Arrange
            bool expected = true;
            bool actual = false;
            //Act
            Account testAccount = new Account("testerEmail", "testHashedPassword", "testSalt");
            ICreateMappingService cms = new SqlCreateMappingService(testAccount, createMappingDAO);
            int userID = cms.CreateMapping();

            if (userID>=0)
            {
                actual = true;
                Console.WriteLine("New ID is: "+userID);
            }

            DeleteMapping(testAccount.UserEmail);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Test to check if the CreateAccoutnService can successfully connect to the database and create mapping row
        //Should return a userID which is >=0;
        [TestMethod]
        public void CreateMappingNotPass()
        {
            //Arrange
            bool expected = false;
            bool actual = false;
            //Act
            Account testAccount = new Account("testerEmail", "testHashedPassword", "testSalt");
            ICreateMappingService cms = new SqlCreateMappingService(testAccount, createMappingDAO);
            cms.CreateMapping();
            int userID = cms.CreateMapping();

            if (userID >=0)
            {
                actual = true;
                Console.WriteLine("New ID is: " + userID);
            }

            DeleteMapping(testAccount.UserEmail);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Test to check if the CreateAccoutnService can successfully connect to the database and create an user row
        [TestMethod]
        public void CreateUserPass()
        {
            //Arrange
            bool expected = true;
            //Act
            Account testAccount = new Account("testerEmail", "testHashedPassword", "testSalt");
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, createAccountDAO);
            DeleteAccount(testAccount.UserEmail);
            cas.Create();
            ICreateMappingService cms = new SqlCreateMappingService(testAccount, createMappingDAO);
            DeleteMapping(testAccount.UserEmail);
            int userID = cms.CreateMapping();
            User testUser = new User(userID, "TesterEmail@testmail.com", "Albert", "Du", "Enable", DateTime.Today,"Male");
            DeleteUser(testUser.UserEmail);
            ICreateUserService cus = new SqlCreateUserService(testUser, createUserDAO);
            bool actual = cus.CreateUser().IsSuccess;
            DeleteUser(testUser.UserEmail);
            DeleteMapping(testAccount.UserEmail);
            DeleteAccount(testAccount.UserEmail);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Test for fail condition When an email/ID is already used, the service should not create a new user again
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void CreateUserNotPass()
        {
            //Act
            Account testAccount = new Account("testerEmail", "testHashedPassword", "testSalt");    
            DeleteMapping(testAccount.UserEmail);
            ICreateMappingService cms = new SqlCreateMappingService(testAccount, createMappingDAO);
            int userID = cms.CreateMapping();
            User testUser = new User(userID, "TesterEmail@testmail.com", "Albert", "Du", "Enable", DateTime.Today, "Male");
            DeleteUser(testUser.UserEmail);
            ICreateUserService cus = new SqlCreateUserService(testUser, createUserDAO);
            cus.CreateUser();
            bool actual = cus.CreateUser().IsSuccess;
            DeleteUser(testUser.UserEmail);
            DeleteMapping(testAccount.UserEmail);
        }

        public void DeleteMapping(string userEmail)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["sqlConnectionMapping"]))
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
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["sqlConnectionAccount"]))
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
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["sqlConnectionSystem"]))
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
