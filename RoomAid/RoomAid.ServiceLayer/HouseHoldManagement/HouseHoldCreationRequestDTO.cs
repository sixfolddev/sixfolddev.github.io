using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer.HouseHoldManagement
{
    public class HouseHoldCreationRequestDTO
    {
        public User User { get; }
        public double Rent { get; }
        public string StreetAddress { get; }
        public int Zip { get; }

        public bool IsAvailable { get; }

        public HouseHoldCreationRequestDTO(User user, double rent, string streetAddress, int zip, bool isAvailable)
        {
            User = user;
            Rent = rent;
            StreetAddress = streetAddress;
            Zip = zip;
            IsAvailable = isAvailable;
        }
    }
}
