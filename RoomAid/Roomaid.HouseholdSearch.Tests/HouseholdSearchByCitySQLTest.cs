using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.ManagerLayer;
using RoomAid.ServiceLayer;
using RoomAid.ServiceLayer.HouseholdSearch;

namespace Roomaid.HouseholdSearch.Tests
{
    [TestClass]
    public class HouseholdSearchByCitySQLTest
    {
        [TestMethod]
        public void searchByCity_pass()
        {
            bool expected = true;

            List<String> filters = new List<string>();
            HouseholdSearchByCitySQL citySearch = new HouseholdSearchByCitySQL("Cypress", 0, filters);
            IResult result = citySearch.Search();
            bool actual = result.IsSuccess;

            Assert.AreEqual(expected, actual);
        }
    }
}
