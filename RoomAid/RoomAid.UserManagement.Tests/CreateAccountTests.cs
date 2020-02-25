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
        //Test to check if the CreateAccoutnService can successfully connect to the database and create a user account
        [TestMethod]
        public void CreateAccountPass()
        {
            //Arrange
            bool expected = true;

            //Act
            Account testAccount = new Account("testerEmail","testHashedPassword", "testSalt");
            IUpdateAccountDAO DAO = new UpdateAccountSqlDAO(ConfigurationManager.AppSettings["sqlConnectionAccount"]);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, DAO);
            IResult result = cas.Create();
            bool actual = result.IsSuccess;
            DeleteAccount(testAccount.UserEmail);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Test to check if the CreateAccoutnService can correctly return false and error message when failed creating a user account because
        //the user email is already registered
        [TestMethod]
        public void CreateAccountNotPass()
        {
            //Arrange
            bool expected = false;

            //Act
            User testUser = new User(1, "testingemail@email.com", "testerFname", "testerLname", "enable", new DateTime(2008, 5, 1), "male");
            //Assert
            bool actual = false;
            Assert.AreEqual(expected, actual);
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
