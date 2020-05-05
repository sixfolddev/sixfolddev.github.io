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
        private readonly IHHListingDAO hhListingDAO;

        public HouseHoldListingService(IHHListingDAO dao)
        {
            this.hhListingDAO = dao;
        }

        public int Insert(int hid)
        {
            throw new NotImplementedException();
        }
        public int Update(int hid, string zipCode, string address, SqlBoolean availability, string hhType, decimal price, string description)
        {
            throw new NotImplementedException();
        }
        public HHListingModel Retrieve(int hid)
        {
            throw new NotImplementedException();
        }
        public int Delete(int hid)
        {
            throw new NotImplementedException();
        }
        
    }
}
