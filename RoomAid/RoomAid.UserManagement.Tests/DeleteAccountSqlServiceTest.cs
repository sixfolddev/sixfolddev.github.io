using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.DataAccessLayer;
using RoomAid.ServiceLayer;

namespace UserManagementTests
{
    [TestClass]
    public class DeleteAccountSqlServiceTest
    {
        private ICreateAccountDAO newAccountDAO = new SqlCreateAccountDAO(ConfigurationManager.AppSettings["sqlConnectionAccount"]);
        private ICreateAccountDAO newMappingDAO = new SqlCreateAccountDAO(ConfigurationManager.AppSettings["sqlConnectionMapping"]);
        private IMapperDAO mapperDAO = new SqlMapperDAO(ConfigurationManager.AppSettings["sqlConnectionMapping"]);
        private ICreateAccountDAO newUserDAO = new SqlCreateAccountDAO(ConfigurationManager.AppSettings["sqlConnectionSystem"]);
        private IDeleteAccountDAO systemDB = new SqlDeleteAccountDAO(ConfigurationManager.AppSettings["sqlConnectionSystem"]);
        private IDeleteAccountDAO mappingDB = new SqlDeleteAccountDAO(ConfigurationManager.AppSettings["sqlConnectionMapping"]);
        private IDeleteAccountDAO accountDB = new SqlDeleteAccountDAO(ConfigurationManager.AppSettings["sqlConnectionAccount"]);

        [TestMethod]
        //Create a new acccount and a matching user. Execute deletion for a single user.
        public void DeleteSingleUser_Pass()
        {
            //Arrange
            bool expected = true;

            Account testAccount = new Account("tester1Email", "testHashedPassword", "testSalt");
            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, daos);
            cas.Create();
            User testUser = new User(mapperDAO.GetSysID(testAccount.UserEmail), "tester1Email", "Collin", "Damarines", "Activated", DateTime.Now, "Male");

            //Act
            IDeleteAccountService deleter = new DeleteAccountSQLService(testUser, systemDB, mappingDB, accountDB);
            IResult deleteResult = deleter.Delete();
            bool actual = deleteResult.IsSuccess;
            Console.WriteLine(deleteResult.Message);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        //Create multiple accounts and users. Execute deletion of multiple users.
        public void DeleteMultipleUsers_Pass()
        {
            //Arrange
            bool expected = true;

            Account testAccount = new Account("tester2Email", "testHashedPassword", "testSalt");
            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, daos);
            cas.Create();
            User testUser = new User(mapperDAO.GetSysID(testAccount.UserEmail), "tester2Email", "Collin", "Damarines", "Activated", DateTime.Now, "Male");

            Account testAccount2 = new Account("tester3Email", "testHashedPassword2", "testSalt");
            cas = new SqlCreateAccountService(testAccount2, daos);
            cas.Create();
            User testUser2 = new User(mapperDAO.GetSysID(testAccount2.UserEmail), "tester3Email", "Woodrow", "Buthavenopaddle", "Activated", DateTime.Now, "Male");

            List<User> testUsers = new List<User>();
            testUsers.Add(testUser);
            testUsers.Add(testUser2);

            //Act
            IDeleteAccountService deleter = new DeleteAccountSQLService(testUsers, systemDB, mappingDB, accountDB);
            IResult deleteResult = deleter.Delete();
            bool actual = deleteResult.IsSuccess;

            //Assert
            Assert.AreEqual(expected, actual);


        }
    }
}
