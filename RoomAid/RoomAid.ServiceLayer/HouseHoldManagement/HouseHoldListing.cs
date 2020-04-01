using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    public class HouseHoldListing
    {
        public int HID { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }


        public HouseHoldListing(int hid, string type, string description, double price)
        {
            HID = hid;
            Price = price;
            Type = type;
            Description = description;
            Price = price;
        }
    }
}
