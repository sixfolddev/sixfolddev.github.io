using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using RoomAid.DataAccessLayer;
using System.Data.SqlClient;

namespace UserManagementTests
{
    [TestClass]
    public class PasswordUpdateSqlServiceTest
    {
        [TestMethod]
        public void UpdateTestSuccess()
        {

        }

        [TestMethod]
        public void UpdateTestFailure()
        {

        }

        [TestMethod]
        public void UpdateTestException()
        {
            var dao = new SqlDAO("false");
            try
            {
                dao.RunCommand(new List<SqlCommand>());
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
