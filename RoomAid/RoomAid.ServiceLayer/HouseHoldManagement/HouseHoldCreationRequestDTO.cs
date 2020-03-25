using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    
    public class HouseHoldCreationRequestDTO
    {
       public User Requester { get; }
       public string StreetAddress { get; }
       public string City { get; }
       public int Zip { get; }
       public string SuiteNumber { get; }
       public double Rent { get; }

        public HouseHoldCreationRequestDTO(User user, string streetAddress, string city, int zip, string suiteNumber, double rent)
        {
            Requester = user;

            StreetAddress = streetAddress;

            City = city;

            Zip = zip;

            SuiteNumber = suiteNumber;

            Rent = rent;
        }
    }
}
