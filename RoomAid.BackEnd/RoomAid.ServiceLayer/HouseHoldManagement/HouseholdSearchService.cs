using RoomAid.DataAccessLayer.HouseHoldManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer.HouseHoldManagement
{
    public class HouseholdSearchService : IHouseholdSearchService
    {
        private IHouseholdSearchDAO _householdSearchDAO;
        public HouseholdSearchService(IHouseholdSearchDAO searchDAO)
        {
            _householdSearchDAO = searchDAO;
        }
        public ICollection<HouseholdSearchDTO> Search(string cityName, int page, int minPrice, int maxPrice, string householdType)
        {
            return _householdSearchDAO.Search(cityName, page, minPrice, maxPrice, householdType);

        }
        public int GetTotalResultCountForQuery(string cityName, int minPrice, int maxPrice, string householdType)
        {
            return _householdSearchDAO.GetTotalResultCountForQuery(cityName, minPrice, maxPrice, householdType);
        }
        public ICollection<string> GetAutocompleteCities()
        {
            return _householdSearchDAO.GetAutocompleteCities();
        }
    }
}
