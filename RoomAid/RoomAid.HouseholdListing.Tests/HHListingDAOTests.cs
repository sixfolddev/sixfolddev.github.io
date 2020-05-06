using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RoomAid.HouseholdListing.Tests
{
    [TestClass]
    public class HHListingDAOTests
    {
        private RoomAid.DataAccessLayer.HouseHoldListing.HHListingDAO dao;
        private readonly string envConnectionString = "sqlConnectionSystemWithDatePosted";
        private readonly int testHID = 012393;

        [TestInitialize]
        public void Setup()
        {
            dao = new DataAccessLayer.HouseHoldListing.HHListingDAO(Environment.GetEnvironmentVariable(envConnectionString, EnvironmentVariableTarget.User));
            var model = new RoomAid.DataAccessLayer.HouseHoldListing.HHListingModel(testHID);
        }

        [TestMethod]
        public void InsertTest_Pass()
        {
            //Arrange
            
            //Act

            //Assert
        }
    }
}
