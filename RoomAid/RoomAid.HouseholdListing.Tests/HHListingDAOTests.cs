using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.DataAccessLayer;
using RoomAid.DataAccessLayer.HouseHoldListing;
using RoomAid.ServiceLayer;
using RoomAid.ServiceLayer.HouseHoldListing;
using System;
using System.Data.SqlTypes;

namespace RoomAid.HouseholdListing.Tests
{
    [TestClass]
    public class HHListingDAOTests
    {
        //DAOs
        private IHHListingDAO listingDAO;
        private IHouseHoldDAO householdDAO ;
        //Services
        private HouseHoldService hhService;
        private HouseHoldListingService hhListingService;

        //Testing Information
        private readonly string envConnectionString = "sqlConnectionSystemWithDatePosted";
        private int _testHouseholdID; //May need changes depending on database.
        #region HouseHold Information
        private readonly double _testRent = 1000.00;
        private readonly int _testZipCodeInt = 92868;
        private readonly string _testZipCodeString = "92868";
        private readonly string _testAddress = "Test Address"; //Note that households acnnot be created with the same address. Therefore, delete the household as cleanup.
        private readonly bool _testAvailabilityTrue = true;
        #endregion

        #region Update HouseholdListing Information
        //HouseholdListings
        private readonly DateTime _testDatePosted = DateTime.UtcNow;
        private readonly decimal _updatePrice = new Decimal(100.00);
        private readonly string _updateHouseholdType = "Mobile Home";
        private readonly string _updateListingDescription = "Empty Test Garbage";

        //Household
        private readonly string _updateZipCode = "90501";
        private readonly string _updateAddress = "Updated Address";
        private readonly SqlBoolean _updateAvailability = false;
        #endregion
        

        /// <summary>
        /// Setup Database Environment with the HouseholdListing Branch's sql data.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            //Arranging DAOs and Services
            listingDAO = new DataAccessLayer.HouseHoldListing.HHListingDAO(Environment.GetEnvironmentVariable(envConnectionString, EnvironmentVariableTarget.User));
            householdDAO = new SqlHouseHoldDAO(Environment.GetEnvironmentVariable(envConnectionString, EnvironmentVariableTarget.User));

            hhService = new HouseHoldService(householdDAO);
            hhListingService = new HouseHoldListingService(listingDAO);

            HouseHold _testHousehold = new RoomAid.ServiceLayer.HouseHold(_testRent, _testAddress, _testZipCodeInt, _testAvailabilityTrue);
            _testHouseholdID = hhService.CreateHouseHold(_testHousehold);
        }

        /// <summary>
        /// Successful test from generating a household, inserting a listing, retrieving the listing, updating the listing, and finally deleting the listing.
        /// If this test does not pass, the rest will most likely not pass.
        /// </summary>
        [TestMethod]
        public void HHLISTINGDAO_CRUD_Test_PASS()
        {
            #region Insert
            //Arrange
            HHListingModel _testModel = new HHListingModel(_testHouseholdID);
            var expectedRowsInserted = 1;
            
            //Act
            var insertResult = listingDAO.Insert(_testModel);

            //Assert
            Assert.AreEqual(expectedRowsInserted, insertResult);
            #endregion

            #region Retrieve
            //Arrange
            var expectedModel= new HHListingModel(_testHouseholdID);
            //Act
            var retrieveModel = listingDAO.Retrieve(_testHouseholdID);

            //Assert
            Assert.AreEqual(expectedModel.HID, retrieveModel.HID);
            Assert.AreEqual(expectedModel.HouseholdType, retrieveModel.HouseholdType);
            Assert.AreEqual(expectedModel.ListingDescription, retrieveModel.ListingDescription);
            Assert.AreEqual(expectedModel.Price, retrieveModel.Price);
            Assert.AreEqual(_testZipCodeString, retrieveModel.ZipCode);
            Assert.AreEqual(_testAvailabilityTrue, retrieveModel.Availability);
            Assert.AreEqual("Not Available", retrieveModel.HostName); //Default value specified in HHListingModel
            #endregion

            #region Update
            #endregion
            //Arrange
            var oldDateTime = retrieveModel.DatePosted;
            var updateModel = new HHListingModel(_testHouseholdID, DateTime.UtcNow, SqlBoolean.False, zipCode: _updateZipCode,  streetAddress: _updateAddress,
                householdType: _updateHouseholdType, listingDescription: _updateListingDescription, price:_updatePrice);
            var expectedRowsUpdated = 1;
            //Act
            //Update shall only be used to update certain fields. shall not update host name.
            var updateResult = listingDAO.Update(updateModel);
             var retrievedUpdateModel = listingDAO.Retrieve(updateModel.HID);
            //Assert
            Assert.AreEqual(expectedRowsUpdated, updateResult);
            //Resources online are available to implement testing date time. previous results show acceptable margins
            //Assert.AreEqual(updateModel.DatePosted, retrievedUpdateModel.DatePosted, TimeSpan.FromMinutes(1));
            Assert.AreEqual(updateModel.HID, retrievedUpdateModel.HID);
            Assert.AreEqual(updateModel.Availability, retrievedUpdateModel.Availability);
            Assert.AreEqual(updateModel.ZipCode, retrievedUpdateModel.ZipCode);
            //Assert.AreEqual(updateModel.StreetAddress, retrievedUpdateModel.StreetAddress); 5/7/20 StreetAddress is updated in database but you cannot retrieve street address using householdListing fetch.
            Assert.AreEqual(updateModel.HouseholdType, retrievedUpdateModel.HouseholdType); //5/7/20 BRD does not specify whether householdType is retrieved. Value is updated in db.
            Assert.AreEqual(updateModel.ListingDescription, retrievedUpdateModel.ListingDescription);
            Assert.AreEqual(updateModel.Price, retrievedUpdateModel.Price);

            #region Delete
            //Arrange
            var expectedRowsDeleted = 1;
            var expectedDeleteModel = new HHListingModel(); //Creates an empty HHListingModel
            //Act Expected delete behavior is that a listing is deleted when a household is deleted. Requires we delete the household.
            var deleteListingResult = listingDAO.Delete(_testHouseholdID);
            var deleteHouseholdResult = hhService.DeleteHouseHold(_testHouseholdID);

            var resultDeleteModel = listingDAO.Retrieve(_testHouseholdID); //retrieving a non existent household should return an empty HHListingModel

            Assert.AreEqual(expectedRowsDeleted, deleteListingResult);
            Assert.AreEqual(expectedDeleteModel.HID, resultDeleteModel.HID);
            Assert.AreEqual(expectedDeleteModel.HostName, resultDeleteModel.HostName);
            Assert.AreEqual(expectedDeleteModel.Availability, resultDeleteModel.Availability);
            Assert.AreEqual(expectedDeleteModel.ZipCode, resultDeleteModel.ZipCode);
            Assert.AreEqual(expectedDeleteModel.StreetAddress, resultDeleteModel.StreetAddress);
            Assert.AreEqual(expectedDeleteModel.HouseholdType, resultDeleteModel.HouseholdType);
            Assert.AreEqual(expectedDeleteModel.ListingDescription, resultDeleteModel.ListingDescription);
            Assert.AreEqual(expectedDeleteModel.Price, resultDeleteModel.Price);
            #endregion
        }

        [TestMethod]
        public void HHListingDAO_Insert_TestDuplicateRecord_FAIL()
        {
            //Arrange
            var expectedRowsChanged1 = 1;
            var expectedRowsChanged2 = 0;
            var insertModel = new HHListingModel(_testHouseholdID);

            //Act
            var resultRowsChanged1 = listingDAO.Insert(insertModel);
            var resultRowsChanged2 = listingDAO.Insert(insertModel);

            //Assert
            Assert.AreEqual(expectedRowsChanged1, resultRowsChanged1);
            Assert.AreEqual(expectedRowsChanged2, resultRowsChanged2);

            //CleanUp
            listingDAO.Delete(_testHouseholdID);
            hhService.DeleteHouseHold(_testHouseholdID);
              
        }

        [TestMethod]
        public void HHListingDAO_Insert_TestEmptyModel_Fail()
        {
            //Arrange
            var expectedRowsChanged = 0;
            var insertModel = new HHListingModel(); //Empty model should have an invalid HID of 0.
            //Act
            var resultRowsChanged = listingDAO.Insert(insertModel);

            //Assert
            Assert.AreEqual(expectedRowsChanged, resultRowsChanged);

            //Cleanup
            hhService.DeleteHouseHold(_testHouseholdID);
        }

        [TestMethod]
        public void HHListingDAO_Insert_TestNonExistentHousehold_Fail()
        {
            //Arrange
            //Cleanup as part of arrange
            hhService.DeleteHouseHold(_testHouseholdID);
            var expectedRowsChanged = 0;
            var insertModel = new HHListingModel(_testHouseholdID); 
            //Act
            var resultRowsChanged = listingDAO.Insert(insertModel);

            //Assert
            Assert.AreEqual(expectedRowsChanged, resultRowsChanged);
        }








    }
}
