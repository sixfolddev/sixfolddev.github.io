using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.DataAccessLayer.HouseHoldManagement;
using RoomAid.ServiceLayer.HouseHoldManagement;

namespace RoomAid.HouseHoldManagement.Tests
{
    [TestClass]
    public class CreateHouseHoldServiceTest
    {
        private IHouseHoldDAO hdao = new SqlHouseHoldDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
        
        //Success condition for HouseHoldCreation
        [TestMethod]
        public void HouseHoldCreatePass()
        {
            //Arrange
            bool expected = true;
            bool actual = false;
            //Act
            HouseHoldService hhs = new HouseHoldService(hdao);
            HouseHoldCreationRequestDTO request = new HouseHoldCreationRequestDTO(null, 1000.00, "Testing address", 92868 , true);
            
            int newID = hhs.CreateHouseHold(request);
            if (newID > 0)
                actual = true;
            Console.WriteLine("New HID is "+newID + " if is not 0 then test pass\nCreateHouseHold will return newID when success");
            hhs.DeleteHouseHold(newID);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        //Failure condition for HouseHoldCreation, when an address with Zip is already used, the new household should not be created
        [TestMethod]
        public void HouseHoldCreateNotPassA()
        {
            //Arrange
            bool expected = false;
            bool actual = false;
            //Act
            HouseHoldService hhs = new HouseHoldService(hdao);
            HouseHoldCreationRequestDTO request = new HouseHoldCreationRequestDTO(null, 1000.00, "Testing address", 92868, true);
            int newHouse1 = hhs.CreateHouseHold(request);
            int newHouse2 = hhs.CreateHouseHold(request);
            if (newHouse2 > 0)
                actual = true;
            Console.WriteLine("First HID is " + newHouse1);
            Console.WriteLine("Second HID is " + newHouse2 + " if is 0 then test pass\nCreateHouseHold will return 0 when this address is already used");
            hhs.DeleteHouseHold(newHouse1);
            //Assert
            Assert.AreEqual(expected, actual);

        }

        //Failure condition for HouseHoldCreation, when the input Zip is not valid should not create new HouseHold
        [TestMethod]
        public void HouseHoldCreateNotPassB()
        {
            //Arrange
            bool expected = false;
            bool actual = false;
            //Act
            HouseHoldService hhs = new HouseHoldService(hdao);
            HouseHoldCreationRequestDTO request = new HouseHoldCreationRequestDTO(null, 1000.00, "Testing address", 999999, true);
            int newID = hhs.CreateHouseHold(request);
            if (newID > 0)
                actual = true;
            Console.WriteLine("New HID is " + newID+ " if is 0 then test pass\nCreateHouseHold will return 0 if this zip is not valid");
            hhs.DeleteHouseHold(newID);
            //Assert
            Assert.AreEqual(expected, actual);

        }
    }
}
