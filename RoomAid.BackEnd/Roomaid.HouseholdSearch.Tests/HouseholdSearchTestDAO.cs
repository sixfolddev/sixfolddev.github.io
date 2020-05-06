using RoomAid.DataAccessLayer.HouseHoldManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roomaid.HouseholdSearch.Tests
{
    /// <summary>
    /// Mock DAO that creates random values in memory that can be passed back to the service.
    /// </summary>
    class HouseholdSearchTestDAO : IHouseholdSearchDAO
    {
        private ICollection<HouseholdSearchDTO> _mockData = new Collection<HouseholdSearchDTO>();
        public HouseholdSearchTestDAO()
        {
            MockDatastore();
        }

        public int GetTotalResultCountForQuery(string cityName, int minPrice, int maxPrice, string householdType)
        {
            return _mockData.Count;
        }

        public ICollection<HouseholdSearchDTO> Search(string cityName, int page, int minPrice, int maxPrice, string householdType)
        {
            return _mockData;
        }

        private void MockDatastore()
        {
            _mockData.Add(TestDAODataGeneration("Apartment", "Roomate Wanted!", 500));
            _mockData.Add(TestDAODataGeneration("Apartment", "Roomate Desperately Wanted!", 1000));
            _mockData.Add(TestDAODataGeneration("House", "Need a few roomates", 800));
            _mockData.Add(TestDAODataGeneration("Townhouse", "Help", 600));
        }

        private HouseholdSearchDTO TestDAODataGeneration(string householdType, string listingDescription, int price)
        {
            HouseholdSearchDTO tempData = new HouseholdSearchDTO();
            tempData.HouseholdType = householdType;
            tempData.ListingDescription = listingDescription;
            tempData.Price = price.ToString();
            return tempData;
        }
    }
}
