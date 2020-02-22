using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementTests
{
    [TestClass]
    public class UpdateAccountSqlDAOTest
    {
        /// <summary>
        /// Integration testing of the update test
        /// </summary>
        [TestMethod]
        public void UpdateTestSuccess()
        {

        }
        /// <summary>
        /// integration testing of the update test
        /// </summary>
        [TestMethod]
        public void UpdateTestFailure()
        {

        }

        [TestMethod]
        public void UpdateTestExceptionThrown()
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
