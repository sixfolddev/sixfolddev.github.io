using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer.HouseholdListingDTO
{
    public class HouseholdListingDTO
    {
        public string HID { get; set; }
        public List<String> Images { get; set; }
        public string HouseholdType { get; set; }
        public string ListingDescription { get; set; }
        public string Price { get; set; }
    }
}
