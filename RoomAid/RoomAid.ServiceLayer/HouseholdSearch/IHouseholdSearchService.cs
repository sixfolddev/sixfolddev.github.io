using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomAid.DataAccessLayer.HouseholdListingDTO;

namespace RoomAid.ServiceLayer
{
    public interface IHouseholdSearchService
    {
        List<HouseholdListingDTO> Search();
    }
}
