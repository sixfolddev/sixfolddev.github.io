using System;
using System.Collections.Generic;
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
       private ICreateAccountDAO newAccountDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User));
       private ICreateAccountDAO newMappingDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
       private  IMapperDAO mapperDAO = new SqlMapperDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
       private ICreateAccountDAO newUserDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));

        //Test to check if the CreateAccountService can successfully connect to the database and create a user account
        [TestMethod]
        public void CreateAccountPass()
        {
            //Arrange
            bool expected = true;
            //Act
            Account testAccount = new Account("testerEmail","testHashedPassword", "testSalt");
            DeleteUser(testAccount.UserEmail);
            DeleteMapping(testAccount.UserEmail);
            DeleteAccount(testAccount.UserEmail);

            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount,daos);
            IResult result = cas.Create();
            bool actual = result.IsSuccess;
            DeleteUser(testAccount.UserEmail);
            DeleteMapping(testAccount.UserEmail);
            DeleteAccount(testAccount.UserEmail);
            Console.WriteLine(result.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Test to check if the CreateAccoutnService can successfully connect to the database and create mutiple user accounts
        [TestMethod]
        public void CreateMultipleAccountPass()
        {
            //Arrange
            bool expected = true;
            //Act
            List<Account> testAccounts = new List<Account>();

            for (int i=0;i<10;i++)
            {
                Account testAccount = new Account("testerEmail"+i, "testHashedPassword", "testSalt");
                testAccounts.Add(testAccount);
                DeleteUser(testAccount.UserEmail);
                DeleteMapping(testAccount.UserEmail);
                DeleteAccount(testAccount.UserEmail);
            }

            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccounts, daos);
            IResult result = cas.Create();
            bool actual = result.IsSuccess;

            foreach (Account testAccount in testAccounts)
            {
                DeleteUser(testAccount.UserEmail);
                DeleteMapping(testAccount.UserEmail);
                DeleteAccount(testAccount.UserEmail);
            }
            
            Console.WriteLine(result.Message);
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
            DeleteUser(testAccount.UserEmail);
            DeleteMapping(testAccount.UserEmail);
            DeleteAccount(testAccount.UserEmail);
            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, daos);
            cas.Create();
            IResult result = cas.Create();
            bool actual = result.IsSuccess;
            DeleteUser(testAccount.UserEmail);
            DeleteMapping(testAccount.UserEmail);
            DeleteAccount(testAccount.UserEmail);
            Console.WriteLine(result.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Test for failure condition of creating multiple Accounts, the system should throw an exception
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void CreateMultipleAccountNotPass()
        {
            //Arrange
            bool expected = false;
            //Act
            List<Account> testAccounts = new List<Account>();

            for (int i = 0; i < 10; i++)
            {
                Account testAccount = new Account("testerEmail" + i, "testHashedPassword", "testSalt");
                testAccounts.Add(testAccount);
                DeleteUser(testAccount.UserEmail);
                DeleteMapping(testAccount.UserEmail);
                DeleteAccount(testAccount.UserEmail);
            }

            testAccounts.Add(new Account("testerEmail" + 0, "testHashedPassword", "testSalt"));


            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccounts, daos);
            IResult result = cas.Create();
            bool actual = result.IsSuccess;

            foreach (Account testAccount in testAccounts)
            {
                DeleteUser(testAccount.UserEmail);
                DeleteMapping(testAccount.UserEmail);
                DeleteAccount(testAccount.UserEmail);
            }

            Console.WriteLine(result.Message);
            //Assert
            Assert.AreEqual(expected, actual);
        }


        //Cleanning tools
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
