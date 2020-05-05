using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomAid.DataAccessLayer.HouseHoldListing;

namespace RoomAid.ServiceLayer.HouseHoldListing
{
    public class HouseHoldListingService
    {
        private readonly IHHListingDAO dao;
        private const int numberOfRetries = 3;

        public HouseHoldListingService(IHHListingDAO dao)
        {
            this.dao = dao;
        }

        public bool Insert(int hid)
        {
            var model = new HHListingModel(hid, DateTime.UtcNow, SqlBoolean.False);
            for (int i = 0; i < numberOfRetries; i++)
            {
                try {
                    var rowsChanged = dao.Insert(model);
                    if (rowsChanged == 1)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    //Should Logging be done for every failed attempt?
                }
            }
            throw new Exception("Error encountered in HouseholdListing Insert");

        }

        public bool Update(int hid, string zipCode, string address, SqlBoolean availability, string hhType, decimal price, string description)
        {
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
                catch (Exception e)
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
                catch (Exception e)
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
                catch (Exception e)
                {
                    //Should Logging be done for every failed attempt?
                }
            }
            throw new Exception("Error encountered in HouseholdListing Delete");
        }
        
    }
}
