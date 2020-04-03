using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.DataAccessLayer;
using RoomAid.ManagerLayer.HouseHoldManagement;
using RoomAid.ServiceLayer;

namespace RoomAid.HouseHoldManagement.Tests
{
    [TestClass]
    public class HouseHoldManagerTests
    {
        private ICreateAccountDAO newAccountDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User));
        private ICreateAccountDAO newMappingDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
        private IMapperDAO mapperDAO = new SqlMapperDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
        private ICreateAccountDAO newUserDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));

        //Success condition for HouseHoldCreation
        [TestMethod]
        public void HouseHoldCreatePass()
        {
            //Arrange
            bool expected = true;
            //Act
            Account testAccount = new Account("testerEmail", "testHashedPassword", "testSalt");
            DeleteUser(testAccount.UserEmail);
            DeleteMapping(testAccount.UserEmail);
            DeleteAccount(testAccount.UserEmail);

            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, daos);
            HouseHoldManager mr = new HouseHoldManager();
            cas.Create();
            int sid = mapperDAO.GetSysID("testerEmail");
            User testUser = new User(sid, "testerEmail", "testerFname", "testerLname", "Enable", DateTime.Today, "Male");
            HouseHoldCreationRequestDTO request = new HouseHoldCreationRequestDTO(testUser, "TestStreetAddress", "TestCity",92868,"TestSuiteNumber", 1500.00);
            IResult result = mr.CreateNewHouseHold(request);
            Console.WriteLine("New HouseHold ID is: " + result.Message);
            int hID = Int32.Parse(result.Message);
            Console.WriteLine("New HouseHold ID is: "+result.Message);
            DeleteUser(testAccount.UserEmail);
            DeleteMapping(testAccount.UserEmail);
            DeleteAccount(testAccount.UserEmail);
            mr.DeleteHouseHold(hID);
            bool actual = result.IsSuccess;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Failure condition for HouseHoldCreation, if the request user does not exist, HouseHold Creation should not start
        [TestMethod]
        public void HouseHoldCreateNotPassA()
        {
            //Arrange
            bool expected = false;
            //Act
            //Create a new testing user and then delete it
            Account testAccount = new Account("testerEmail", "testHashedPassword", "testSalt");
            DeleteUser(testAccount.UserEmail);
            DeleteMapping(testAccount.UserEmail);
            DeleteAccount(testAccount.UserEmail);

            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, daos);
            HouseHoldManager mr = new HouseHoldManager();
            cas.Create();
            int sid = mapperDAO.GetSysID("testerEmail");

            DeleteUser(testAccount.UserEmail);
            DeleteMapping(testAccount.UserEmail);
            DeleteAccount(testAccount.UserEmail);

            User testUser = new User(sid, "testerEmail", "testerFname", "testerLname", "Enable", DateTime.Today, "Male");
            HouseHoldCreationRequestDTO request = new HouseHoldCreationRequestDTO(testUser, "TestStreetAddress", "TestCity", 92868, "TestSuiteNumber", 1500.00);
            IResult result = mr.CreateNewHouseHold(request);
            Console.WriteLine(result.Message);
            bool actual = result.IsSuccess;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Failure condition for HouseHoldCreation, if the HouseHold Already exist, should now create this Household
        [TestMethod]
        public void HouseHoldCreateNotPassB()
        {
            //Arrange
            bool expected = false;
            //Act
            //Create a new testing user and then delete it
            Account testAccount = new Account("testerEmail", "testHashedPassword", "testSalt");
            DeleteUser(testAccount.UserEmail);
            DeleteMapping(testAccount.UserEmail);
            DeleteAccount(testAccount.UserEmail);

            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, daos);
            HouseHoldManager mr = new HouseHoldManager();
            cas.Create();
            int sid = mapperDAO.GetSysID("testerEmail");

            User testUser = new User(sid, "testerEmail", "testerFname", "testerLname", "Enable", DateTime.Today, "Male");
            HouseHoldCreationRequestDTO request = new HouseHoldCreationRequestDTO(testUser, "TestStreetAddress", "TestCity", 92868, "TestSuiteNumber", 1500.00);
            IResult result = mr.CreateNewHouseHold(request);
            int hID = Int32.Parse(result.Message);
            result = mr.CreateNewHouseHold(request);
            Console.WriteLine(result.Message);
            DeleteUser(testAccount.UserEmail);
            DeleteMapping(testAccount.UserEmail);
            DeleteAccount(testAccount.UserEmail);
            mr.DeleteHouseHold(hID);
            bool actual = result.IsSuccess;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Failure condition for HouseHoldCreation, if the input zip is out of CA then it should not create the HouseHold
        [TestMethod]
        public void HouseHoldCreateNotPassC()
        {
            //Arrange
            bool expected = false;
            //Act
            //Create a new testing user and then delete it
            Account testAccount = new Account("testerEmail", "testHashedPassword", "testSalt");
            DeleteUser(testAccount.UserEmail);
            DeleteMapping(testAccount.UserEmail);
            DeleteAccount(testAccount.UserEmail);

            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, daos);
            HouseHoldManager mr = new HouseHoldManager();
            cas.Create();
            int sid = mapperDAO.GetSysID("testerEmail");

            User testUser = new User(sid, "testerEmail", "testerFname", "testerLname", "Enable", DateTime.Today, "Male");
            //using zip for New York
            HouseHoldCreationRequestDTO request = new HouseHoldCreationRequestDTO(testUser, "TestStreetAddress", "TestCity", 10001, "TestSuiteNumber", 1500.00);
            IResult result = mr.CreateNewHouseHold(request);
            Console.WriteLine(result.Message);
            DeleteUser(testAccount.UserEmail);
            DeleteMapping(testAccount.UserEmail);
            DeleteAccount(testAccount.UserEmail);
            bool actual = result.IsSuccess;

            //Assert
            Assert.AreEqual(expected, actual);
        }
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
