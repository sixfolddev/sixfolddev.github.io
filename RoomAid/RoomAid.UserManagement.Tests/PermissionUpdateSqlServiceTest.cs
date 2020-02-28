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
                var permUpdate = new PermissionUpdateSqlService(dao, new Permission(-1));
            }
            catch(Exception)
            {

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
