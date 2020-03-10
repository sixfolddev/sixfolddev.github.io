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

        [TestMethod]
        //Create a new acccount and a matching user. Execute deletion for a single user.
        public void DeleteSingleUser_Pass()
        {
            //Arrange
            bool expected = true;

            Account testAccount = new Account("testerEmail", "testHashedPassword", "testSalt");
            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, daos);
            cas.Create();
            User testUser = new User(mapperDAO.GetSysID(testAccount.UserEmail), "testerEmail", "Collin", "Damarines", "Activated", DateTime.Now, "Male");
           
            //Act
            IDeleteAccountDAO systemDB = new SqlDeleteAccountDAO(ConfigurationManager.AppSettings["sqlConnectionSystem"]);
            IDeleteAccountDAO mappingDB = new SqlDeleteAccountDAO(ConfigurationManager.AppSettings["sqlConnectionMapping"]);
            IDeleteAccountDAO accountDB = new SqlDeleteAccountDAO(ConfigurationManager.AppSettings["sqlConnectionAccount"]);
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

        }
    }
}
