using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using RoomAid.ServiceLayer;
using RoomAid.DataAccessLayer;

namespace UserManagementTests
{
    [TestClass]
    public class PermissionUpdateSqlServiceTest
    {

        private ICreateAccountDAO newAccountDAO = new SqlCreateAccountDAO(ConfigurationManager.AppSettings["sqlConnectionAccount"]);
        private ICreateAccountDAO newMappingDAO = new SqlCreateAccountDAO(ConfigurationManager.AppSettings["sqlConnectionMapping"]);
        private IMapperDAO mapperDAO = new SqlMapperDAO(ConfigurationManager.AppSettings["sqlConnectionMapping"]);
        private ICreateAccountDAO newUserDAO = new SqlCreateAccountDAO(ConfigurationManager.AppSettings["sqlConnectionSystem"]);

        [TestMethod]
        public void UpdateTestSuccess()
        {
            Account testAccount = new Account("testerEmail", "testHashedPassword", "testSalt");
            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, daos);
            IResult result = cas.Create();
            Assert.IsTrue(result.IsSuccess);

            var dao = new UpdateAccountSqlDAO(ConfigurationManager.AppSettings["sqlConnectionSystem"]);
            try
            {
                var id = mapperDAO.GetSysID("testerEmail");
                var perm = new Permission(id);
                perm.AddPermission("None");
                var permUpdate = new PermissionUpdateSqlService(dao, perm);
                var res = permUpdate.Update();
                Assert.IsTrue(res.IsSuccess);


            }
            catch
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void UpdateTestFailure()
        {
            var dao = new UpdateAccountSqlDAO(ConfigurationManager.AppSettings["sqlConnectionSystem"]);
            try
            {
                var permission = new Permission(-1);
                permission.AddPermission("None");
                var permUpdate = new PermissionUpdateSqlService(dao,permission);
                var result =permUpdate.Update();
                Assert.Equals(result.IsSuccess, true);
            }
            catch(Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void UpdateTestException()
        {
            var dao = new UpdateAccountSqlDAO("false");
            try
            {
                dao.Update(new List<SqlCommand>());
                Assert.Fail();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                Assert.IsInstanceOfType(e, typeof(Exception));
            }
        }
    
    }
}
