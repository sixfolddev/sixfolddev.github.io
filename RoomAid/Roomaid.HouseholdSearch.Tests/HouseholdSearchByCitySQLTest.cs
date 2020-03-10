using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.DataAccessLayer.HouseholdListingDTO;
using RoomAid.ManagerLayer;
using RoomAid.ServiceLayer;
using RoomAid.ServiceLayer.HouseholdSearch;

namespace Roomaid.HouseholdSearch.Tests
{
    [TestClass]
    public class HouseholdSearchByCitySQLTest
    {
        // TODO: Create generic database placeholder values for testing purposes
        /// <summary>
        /// Enter a valid search criteria: "Cypress" that is currently in the database. Expect back 2 results.
        /// </summary>
        [TestMethod]
        public void searchByCity_pass()
        {
            //Arrange
            bool expected = true;
            bool actual = false;
            //Act
            List<String> filters = new List<string>();
            HouseholdSearchByCitySQL citySearch = new HouseholdSearchByCitySQL("Cypress", 0, filters);
            List<HouseholdListingDTO> returnValues = citySearch.Search();
            if (returnValues.Count == 2)
                actual = true;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Enter a valid search criteria: "Boston" that is not in the database. Expect back 0 results.
        /// </summary>
        [TestMethod]
        public void searchByInvalidCity_fail()
        {
            //Arrange
            bool expected = false;
            bool actual = true;
            //Act
            List<String> filters = new List<string>();
            HouseholdSearchByCitySQL citySearch = new HouseholdSearchByCitySQL("Boston", 0, filters);
            List<HouseholdListingDTO> returnValues = citySearch.Search();
            if (returnValues.Count == 0)
                actual = false;
            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
