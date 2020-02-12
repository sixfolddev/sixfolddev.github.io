using System;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.ServiceLayer;

namespace RoomAid.CreateAccount.Tests
{
    [TestClass]
    public class CreateAccountTests
    {
        [TestMethod]
        public void CreateAccountPass()
        {
            //Arrange
            bool expected = true;

            //Act
            User testUser = new User("testingemail@email.com","testerFname", "testerLname","enable", new DateTime(2008, 5, 1),"male");
            CreateAccountService cas = new CreateAccountService();
            if (cas.IfUserExist(testUser.UserEmail))
                DeleteRow(testUser.UserEmail);

            IResult testResult = cas.CreateAccount(testUser);
            Console.WriteLine(testResult.Message);

            bool actual = testResult.IsSuccess;
            DeleteRow(testUser.UserEmail);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CreateAccountNotPass()
        {
            //Arrange
            bool expected = false;

            //Act
            User testUser = new User("testingemail@email.com", "testerFname", "testerLname", "enable", new DateTime(2008, 5, 1), "male");
            CreateAccountService cas = new CreateAccountService();
            if (!cas.IfUserExist(testUser.UserEmail))
                cas.CreateAccount(testUser);

            IResult testResult = cas.CreateAccount(testUser);
            Console.WriteLine(testResult.Message);

            bool actual = testResult.IsSuccess;
            DeleteRow(testUser.UserEmail);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        public void DeleteRow(string userEmail)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["sqlConnection"]))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM Users Where UserEmail = @userEmail", connection);
                    command.Parameters.AddWithValue("@userEmail", userEmail);
                    using (command)
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.Message);
            }
        
        }
    }
}
