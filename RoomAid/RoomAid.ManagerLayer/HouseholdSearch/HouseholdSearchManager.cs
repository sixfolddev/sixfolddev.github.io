using RoomAid.ServiceLayer;
using RoomAid.ServiceLayer.HouseholdSearch;
using System;
using System.Collections.Generic;

namespace RoomAid.ManagerLayer
{
    public class HouseholdSearchManager
    {
        private IHouseholdSearchService _searchService;
        //TODO: Connect to controller for UI
        public HouseholdSearchManager(string cityName, int lastHID, List<String> filters)
        {
            _searchService = new HouseholdSearchByCitySQL(cityName, lastHID, filters);
            _searchService.Search();
        }
    }
}
