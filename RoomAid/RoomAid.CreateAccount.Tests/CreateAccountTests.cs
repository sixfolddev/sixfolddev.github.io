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
        //Test to check if the CreateAccoutnService can successfully connect to the database and create a user account
        [TestMethod]
        public void CreateAccountPass()
        {
            //Arrange
            bool expected = true;

            //Act
            User testUser = new User("testingemail@email.com","testingPassword","testingSalt","testerFname", "testerLname","enable", new DateTime(2008, 5, 1),"male");
            ICreateAccountService cas = new SqlCreateAccountService();
            if (cas.IfExist(testUser))
                DeleteAccount(testUser.UserEmail);

            IResult testResult = cas.Create(testUser);
            Console.WriteLine(testResult.Message);

            bool actual = testResult.IsSuccess;
            DeleteAccount(testUser.UserEmail);

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
            User testUser = new User("testingemail@email.com", "testingPassword", "testingSalt", "testerFname", "testerLname", "enable", new DateTime(2008, 5, 1), "male");
            ICreateAccountService cas = new SqlCreateAccountService();
            if (!cas.IfExist(testUser))
                cas.Create(testUser);

            IResult testResult = cas.Create(testUser);
            Console.WriteLine(testResult.Message);

            bool actual = testResult.IsSuccess;
            DeleteAccount(testUser.UserEmail);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Tool method to clean testing account created by the test method
        public void DeleteAccount(string userEmail)
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
