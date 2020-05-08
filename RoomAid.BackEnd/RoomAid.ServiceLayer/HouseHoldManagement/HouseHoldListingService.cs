using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using RoomAid.DataAccessLayer.HouseHoldListing;

namespace RoomAid.ServiceLayer.HouseHoldManagement
{
    public class HouseHoldListingService
    {
        private readonly IHHListingDAO dao;
        private const int numberOfRetries = 3;
        /*TODO:
         *  Add Authorization Element AuthZService
         *  Express which claims are necessary for Insert,Update ,Retrieve, Delete
         *  Insert 
         *      Meant to be an automatic inclusion in the household creation process.
         *  Retrieve:
         *      Requires search privileges to match.
         *  Update:
         *      Requires householdID and update privileges to match.
         *  Delete requires some form of identification to match with the household being deleted.
         *      If AuthZClaims HouseholdID and delete privileges match, delete is permitted.
         */
        public HouseHoldListingService(IHHListingDAO dao)
        {
            this.dao = dao;
        }

        public bool Insert(int hid)
        {
            var model = new HHListingModel(hid, DateTime.UtcNow, SqlBoolean.False);
            for (int i = 0; i < numberOfRetries; i++)
            {
                try
                {
                    var rowsChanged = dao.Insert(model);
                    if (rowsChanged == 1)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    //Should Logging be done for every failed attempt?
                }
            }
            throw new Exception("Error encountered in HouseholdListing Insert");

        }

        public bool Update(int hid, string zipCode, string address, SqlBoolean availability, string hhType, decimal price, string description)
        {
            try
            {
                if (!ValidHouseholdTypeSize(hhType) | !ValidZipCodeSize(zipCode) | !ValidStreetAddressSize(address) | !ValidListingDescription(description))
                {
                    throw new ArgumentException("At least one input is invalid for HouseholdListing Update");
                }
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (Exception)
            {
                throw new Exception("Error encountered in HouseholdUpdate input validation");
            }

            var model = new HHListingModel(hid, DateTime.UtcNow, availability, zipCode: zipCode, streetAddress: address, householdType: hhType, listingDescription: description, price: price);
            for (int i = 0; i < numberOfRetries; i++)
            {
                try
                {
                    var rowsChanged = dao.Update(model);
                    if (rowsChanged == 1)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    //Should Logging be done for every failed attempt?
                }
            }

            throw new Exception("Error encountered in HouseholdListing Update");
        }

        public HHListingModel Retrieve(int hid)
        {
            for (int i = 0; i < numberOfRetries; i++)
            {
                try
                {
                    var model = dao.Retrieve(hid);
                    return model;
                }
                catch (Exception)
                {
                    //Should Logging be done for every failed attempt?
                }
            }
            throw new Exception("Error encountered in HouseholdListing Retrieve");

        }
        public bool Delete(int hid)
        {
            for (int i = 0; i < numberOfRetries; i++)
            {
                try
                {
                    var rowsChanged = dao.Delete(hid);
                    if (rowsChanged == 1)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    //Should Logging be done for every failed attempt?
                }
            }
            throw new Exception("Error encountered in HouseholdListing Delete");
        }

        #region Validation Functions for input parameters based on HouseholdListingModel size constraints.
        private bool ValidHostNameSize(string input)
        {
            if (input.Length <= HHListingModel.maxHostName)
            {
                return true;
            }
            return false;
        }
        private bool ValidZipCodeSize(string input)
        {
            if (input.Length <= HHListingModel.maxZipCode)
            {
                return true;
            }
            return false;
        }
        private bool ValidStreetAddressSize(string input)
        {
            if (input.Length <= HHListingModel.maxStreetAddress)
            {
                return true;
            }
            return false;
        }
        private bool ValidHouseholdTypeSize(string input)
        {
            if (input.Length <= HHListingModel.maxHouseholdType)
            {
                return true;
            }
            return false;
        }
        private bool ValidListingDescription(string input)
        {
            if (input.Length <= HHListingModel.maxListingDescription)
            {
                return true;
            }
            return false;
        }
        #endregion    }
    }
