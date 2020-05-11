using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.DataAccessLayer.HouseHoldManagement;
using RoomAid.ManagerLayer.HouseHoldManagement;
using RoomAid.ServiceLayer.HouseHoldManagement;

namespace Roomaid.HouseholdSearch.Tests
{
    [TestClass]
    public class HouseholdSearchTests
    {
        //UNIT TESTS

        /// <summary>
        /// Test methods that pass in a mock DAO that simply returns values to the service. Used to show that DAO's can be interchanged within this system.
        /// The mockDAO object creates for DTO's in memory that it adds to a list and returns.
        /// THe Service will search the "DAO" and receive an ICollection. *Expects 4 values from mockDAO*
        /// </summary>
        [TestMethod]
        public void Search_MockDAOExtensibilityTest_Pass()
        {
            //Arrange
            var actual = false;
            var expected = true;
            IHouseholdSearchDAO mockDAO = new HouseholdSearchTestDAO();
            IHouseholdSearchService mockService = new HouseholdSearchService(mockDAO);
            //Act
            ICollection<HouseholdSearchDTO> mockResultListing = mockService.Search("Cypress", 1, 0, 1000, "none");
            //Assert
            if (mockResultListing.Count == 4)
                actual = true;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test method for testing reusability of service code. MockDAO simply returns a count of its dataset. *Expect count of 4*
        /// </summary>
        [TestMethod]
        public void GetTotalResultCountForQuery_MockDAOExtensibilityTest_Pass()
        {
            //Arrange
            var actual = false;
            var expected = true;
            IHouseholdSearchDAO mockDAO = new HouseholdSearchTestDAO();
            IHouseholdSearchService mockService = new HouseholdSearchService(mockDAO);
            //Act
            int mockResultListing = mockService.GetTotalResultCountForQuery("Cypress", 0, 1000, "none");
            //Assert
            if (mockResultListing == 4)
                actual = true;
            Assert.AreEqual(expected, actual);
        }

        //INTEGRATION TESTS
        /// <summary>
        /// Used to instantiate HouseholdSearchService, HouseholdSearchManager, and HouseholdSearchDAO similar to how the HouseholdSearchController would.
        /// </summary>
        private HouseholdSearchManager instantiateLayers()
        {
            IHouseholdSearchDAO searchDAO = new HouseholdSearchDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
            IHouseholdSearchService searchService = new HouseholdSearchService(searchDAO);
            HouseholdSearchManager searchManager = new HouseholdSearchManager(searchService);
            return searchManager;
        }

        /// <summary>
        /// Search all available households in "Los Angeles". *Expect 20 values (a full page) from test data*
        /// </summary>
        [TestMethod]
        public void Search_SearchByCityWithoutFilters_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            var searchManager = instantiateLayers();
            //Act
            ICollection<HouseholdSearchDTO> resultListing = searchManager.Search("Los Angeles", 1, 0, 10000, "none");
            //Assert
            if (resultListing.Count == 20)
                actual = true;
            Assert.AreEqual(actual, expected);
        }

        /// <summary>
        /// Search with only a type filter "Apartment" on the city "Cypress" . *Expect only 1 value from test data*
        /// </summary>
        [TestMethod]
        public void Search_SearchByCityWithTypeFilter_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            var searchManager = instantiateLayers();
            //Act
            ICollection<HouseholdSearchDTO> resultListing = searchManager.Search("Cypress", 1, 0, 10000, "Apartment");
            //Assert
            if (resultListing.Count == 1)
                actual = true;
            Assert.AreEqual(actual, expected);
        }

        /// <summary>
        /// Tests search within the price range 500 to 1000 in the city Los Angeles. *Expect 11 values from test data*
        /// </summary>
        [TestMethod]
        public void Search_SearchByCityWithPriceFilters_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            var searchManager = instantiateLayers();
            //Act
            ICollection<HouseholdSearchDTO> resultListing = searchManager.Search("Los Angeles", 1, 500, 1000, "none");
            //Assert
            if (resultListing.Count == 11)
                actual = true;
            Assert.AreEqual(actual, expected);
        }

        /// <summary>
        /// Test a search of "Los Angeles" with both price and type filters (500 minPrice, 2000 maxPrice, "Townhouse" type). *Expect 7 results from test data*
        /// </summary>
        [TestMethod]
        public void Search_SearchByCityWithAllFilters_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            var searchManager = instantiateLayers();
            //Act
            ICollection<HouseholdSearchDTO> resultListing = searchManager.Search("Los Angeles", 1, 500, 2000, "Townhouse");
            //Assert
            if (resultListing.Count == 7)
                actual = true;
            Assert.AreEqual(actual, expected);
        }

        /// <summary>
        /// Search for all households in "Los Angeles" page 2. *Expect 3 results from test data*
        /// </summary>
        [TestMethod]
        public void Search_SearchByCityPageTwo_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            var searchManager = instantiateLayers();
            //Act
            ICollection<HouseholdSearchDTO> resultListing = searchManager.Search("Los Angeles", 2, 0, 10000, "none");
            //Assert
            if (resultListing.Count == 3)
                actual = true;
            Assert.AreEqual(actual, expected);
        }

        /// <summary>
        /// Search for households in the city "Los Altos" which does not yet exist in the database. *Expect empty collection*
        /// </summary>
        [TestMethod]
        public void Search_SearchByCityNoResultsFound_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            var searchManager = instantiateLayers();
            //Act
            ICollection<HouseholdSearchDTO> resultListing = searchManager.Search("Los Altos", 1, 0, 10000, "none");
            //Assert
            if (!(resultListing.Any()))
                actual = true;
            Assert.AreEqual(actual, expected);
        }

        /// <summary>
        /// Test to see if the Search functionality when searching for the full 20 values can perform a search in under 3 seconds.
        /// </summary>
        [TestMethod]
        public void Search_PerformanceTest_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            var searchManager = instantiateLayers();
            Stopwatch sw = new Stopwatch();
            //Act
            sw.Start();
            ICollection<HouseholdSearchDTO> resultListing = searchManager.Search("Los Angeles", 1, 0, 10000, "none");
            sw.Stop();
            //Assert
            if (sw.ElapsedMilliseconds <= 3000 && resultListing.Count == 20)
                actual = true;
            TestContext.WriteLine("Elapsed Time(milliseconds): " + sw.ElapsedMilliseconds.ToString());
            Assert.AreEqual(actual, expected);
        }
        /// <summary>
        /// Test ability to get total number of results for pagination *expect 23 from test data*
        /// </summary>
        [TestMethod]
        public void GetTotalResultCountForQuery_GetCountForNumberOfPages_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            var searchManager = instantiateLayers();
            //Act
            var resultCount = searchManager.GetTotalResultCountForQuery("Los Angeles", 0, 10000, "none");
            //Assert
            if (resultCount == 23)
                actual = true;
            Assert.AreEqual(actual, expected);

        }
        /// <summary>
        /// Queries with zero results should return a count of zero
        /// </summary>
        [TestMethod]
        public void GetTotalResultCountForQuery_GetCountForNumberOfPagesNoResults_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            var searchManager = instantiateLayers();
            //Act
            var resultCount = searchManager.GetTotalResultCountForQuery("Los Altos", 0, 10000, "none");
            //Assert
            if (resultCount == 0)
                actual = true;
            Assert.AreEqual(actual, expected);

        }
        /// <summary>
        /// Tests method for getting complete list of cities for autocomplete. *Returns 1238 values from database*
        /// </summary>
        [TestMethod]
        public void GetAutocompleteCities_GetTotalListOfCities_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            var searchManager = instantiateLayers();
            //Act
            var cities = searchManager.GetAutocompleteCities();
            //Assert
            if (cities.Count  == 1238)
                actual = true;
            Assert.AreEqual(actual, expected);
        }
        /// <summary>
        /// Used to display time taken during performance test in the Test Detail Summary
        /// </summary>
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }


    }
}
