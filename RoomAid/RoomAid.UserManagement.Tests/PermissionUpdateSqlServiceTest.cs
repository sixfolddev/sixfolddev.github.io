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
        private readonly IUpdateAccountDAO _dao = new UpdateAccountSqlDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
        private ICreateAccountDAO newAccountDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User));
        private ICreateAccountDAO newMappingDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
        private IMapperDAO mapperDAO = new SqlMapperDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
        private ICreateAccountDAO newUserDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));

        [TestMethod]
        public void UpdateTestSuccess()
        {
            DeleteDummyAccount();
            

            try
            {
                var id = CreateDummyAccount();
                var permission = new Permission(id);
                permission.AddPermission("None");
                var permUpdate = new PermissionUpdateSqlService(_dao, permission);
                var result = permUpdate.Update();


                Assert.AreEqual(result.IsSuccess, true);
                var permission2 = new Permission(id);
                permission2.RemovePermission("None");
                var permUpdate2 = new PermissionUpdateSqlService(_dao, permission2);
                var result2 = permUpdate2.Update();
                Assert.AreEqual(result2.IsSuccess, true);

                DeleteDummyAccount();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                Assert.Fail();
            }

        }

        //returns an exception because it attempts to delete an entry that does not exist
        [TestMethod]
        public void UpdateTestFailure()
        {
            DeleteDummyAccount();
            CreateDummyAccount();

            try
            {
                var permission = new Permission(-1);
                permission.AddPermission("None");
                var permUpdate = new PermissionUpdateSqlService(_dao,permission);
                var result =permUpdate.Update();

               

                Assert.Fail();
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.ToString());
                DeleteDummyAccount();
            }

     

        }

        //[TestMethod]
        //public void UpdateTestException()
        //{
        //    var dao = new UpdateAccountSqlDAO("false");
        //    var permUpdate = new PermissionUpdateSqlService(dao, new Permission(-1));
        //    try
        //    {
        //        permUpdate.Update();
        //        Assert.Fail();
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.WriteLine(e.ToString());
        //        Assert.IsInstanceOfType(e, typeof(Exception));
        //    }
        //}


        private int CreateDummyAccount()
        {
            Account testAccount = new Account("boi@gmail.com", "testHashedPassword", "testSalt");
            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO, mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(testAccount, daos);
            cas.Create();
            return daos.MapperDAO.GetSysID("boi@gmail.com");
        }

        private void DeleteDummyAccount()
        {
            DeleteUser("boi@gmail.com");
            DeleteMapping("boi@gmail.com");
            DeleteAccount("boi@gmail.com");
        }



        //Cleanning tools
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
