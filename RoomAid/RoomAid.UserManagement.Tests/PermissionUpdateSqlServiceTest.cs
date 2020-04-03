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
        private readonly IUpdateAccountDAO _dao = new UpdateAccountSqlDAO(ConfigurationManager.AppSettings["sqlConnectionSystem"]);

        [TestMethod]
        public void UpdateTestSuccess()
        {
            
            try
            {
                var permission = new Permission(0);
                permission.AddPermission("None");
                var permUpdate = new PermissionUpdateSqlService(_dao, permission);
                var result = permUpdate.Update();
                Assert.Equals(result.IsSuccess, true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void UpdateTestFailure()
        {
            try
            {
                var permission = new Permission(-1);
                permission.AddPermission("None");
                var permUpdate = new PermissionUpdateSqlService(_dao,permission);
                var result =permUpdate.Update();
                Assert.Equals(result.IsSuccess, false);
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
            var permUpdate = new PermissionUpdateSqlService(dao, new Permission(-1));
            try
            {
                permUpdate.Update();
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
