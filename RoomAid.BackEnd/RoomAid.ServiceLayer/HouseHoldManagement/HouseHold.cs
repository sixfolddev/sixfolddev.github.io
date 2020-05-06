using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    public class HouseHold
    {
        public int HID { get; set; }
        public double Rent { get; set; }
        public string StreetAddress { get; set; }
        public int Zip { get; set; }

        public bool IsAvailable { get; set; }

        public HouseHold(int hid, double rent, string streetAddress, int zip, bool isAvailable)
        {
            HID = hid;
            Rent = rent;
            StreetAddress = streetAddress;
            Zip = zip;
            IsAvailable = isAvailable;
        }

        //For new HouseHold that are not created yet, no HID will be assigned
        public HouseHold(double rent, string streetAddress, int zip, bool isAvailable)
        {
            HID = 0;
            Rent = rent;
            StreetAddress = streetAddress;
            Zip = zip;
            IsAvailable = isAvailable;
        }
    }
}
