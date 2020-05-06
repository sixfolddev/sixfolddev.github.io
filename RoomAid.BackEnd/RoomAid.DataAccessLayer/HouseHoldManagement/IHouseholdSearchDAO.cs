using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer.HouseHoldManagement
{
    public interface IHouseholdSearchDAO
    {
        ICollection<HouseholdSearchDTO> Search(string cityName, int page, int minPrice, int maxPrice, string householdType);
        int GetTotalResultCountForQuery(string cityName, int minPrice, int maxPrice, string householdType);
    }
}
