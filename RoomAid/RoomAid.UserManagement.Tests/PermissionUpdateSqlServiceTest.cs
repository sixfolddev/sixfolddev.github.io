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
        [TestMethod]
        public void UpdateTestSuccess()
        {

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
