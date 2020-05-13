using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    
    public class HouseholdCreationRequestDTO
    {
       public string Requester { get; set; }
       public string StreetAddress { get; set; }
       public string City { get; set; }
       public int Zip { get; set; }
       public string SuiteNumber { get; set; }
       public double Rent { get; set; }

    }
}
