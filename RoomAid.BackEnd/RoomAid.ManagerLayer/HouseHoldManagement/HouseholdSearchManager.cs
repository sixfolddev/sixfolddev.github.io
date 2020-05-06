using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomAid.DataAccessLayer.HouseHoldManagement;
using RoomAid.ServiceLayer.HouseHoldManagement;

namespace RoomAid.ManagerLayer.HouseHoldManagement
{
    public class HouseholdSearchManager
    {
        private readonly IHouseholdSearchService _searchService;
        //TODO: Include logging parameter
        public HouseholdSearchManager(IHouseholdSearchService searchService)
        {
            _searchService = searchService;
        }
        /// <summary>
        /// Search method for households routed through the HouseholdSearchService. Returns Icollection of HouseholdSearchDTO's
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="page"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="householdType"></param>
        /// <returns></returns>
        public ICollection<HouseholdSearchDTO> Search(string cityName, int page, int minPrice, int maxPrice, string householdType)
        {
            try
            {
                ICollection<HouseholdSearchDTO> resultsListing = _searchService.Search(cityName, page, minPrice, maxPrice, householdType);
                //TODO: Log Search
                return resultsListing;
            }
            catch (Exception e)
            {
                //TODO: Log failure
                throw e;
            }
        }
        /// <summary>
        /// Get total number of results for a particular query for pagination. Returns integer value
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="householdType"></param>
        /// <returns></returns>
        public int GetTotalResultCountForQuery(string cityName, int minPrice, int maxPrice, string householdType)
        {
            return _searchService.GetTotalResultCountForQuery(cityName, minPrice, maxPrice, householdType);
        }
    }
}
