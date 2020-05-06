using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer.HouseHoldManagement
{
    public class HouseholdSearchDTO
    {
        public ICollection<String> Images { get; set; }
        public string HouseholdType { get; set; }
        public string ListingDescription { get; set; }
        public string Price { get; set; }
        public string HostName { get; set; }
    }
}
