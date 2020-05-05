using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer.HouseHoldListing
{
    public class HHListingDAO
    {
        private readonly string _dbConnectionString;

        HHListingDAO(string connection)
        {
            this._dbConnectionString = connection;
        }

        public int Insert(HHListingModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(HHListingModel model)
        {
            throw new NotImplementedException();
        }

        public int Delete(int hid)
        {
            throw new NotImplementedException();
        }

        public int Retrieve(int hid)
        {
            throw new NotImplementedException();
        }
    }
}
