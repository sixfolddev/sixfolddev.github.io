using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomAid.DataAccessLayer.HouseholdListingDTO;

namespace RoomAid.ServiceLayer.HouseholdSearch
{
    public class HouseholdSearchByCitySQL : IHouseholdSearchService
    {
        // User input
        private string _city;
        // Keyset used for pagination
        private int _lastHID;
        // Search filters
        private readonly List<String> _filters;
        // BUSINESS RULE: Return 20 results per page
        private readonly int _limit = 20;
        private List<HouseholdListingDTO> _returnValues = new List<HouseholdListingDTO>();
        // TODO: Generalize connection string for use on all machines
        private string _db = "Data Source=DESKTOP-9ROILOO;Initial Catalog = Roomaid System Database; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        /// <summary>
        /// Keyset pagination requires the id of the last searched item, provided by the client.
        /// The cityName is the input from the user directly.
        /// Filters can be set by the user but aren't required.
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="lastHID"></param>
        /// <param name="filters"></param>
        public HouseholdSearchByCitySQL(string cityName, int lastHID, List<string> filters)
        {
            this._city = cityName;
            this._lastHID = lastHID;
            this._filters = filters;
        }
        // TODO: Utilize data access objects, generate queries here
        public List<HouseholdListingDTO> Search()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_db))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(
                        "SELECT TOP " + this._limit + " * FROM HouseholdListings WHERE HID IN (SELECT HID FROM Households WHERE ZipCode IN(SELECT ZipCode FROM ZipLocations WHERE City = \'" + this._city + "\') AND IsAvailable = 1) AND HID > " + this._lastHID + " ORDER BY HID ASC;");
                    command.Connection = conn;
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            HouseholdListingDTO listing = new HouseholdListingDTO();
                            listing.HID = (dr["HID"].ToString());
                            listing.HouseholdType = dr["HouseholdType"].ToString();
                            listing.ListingDescription = dr["ListingDescription"].ToString();
                            listing.Price = (dr["Price"].ToString());
                            _returnValues.Add(listing);
                        }
                    }
                    conn.Close();
                }
            }
            catch(Exception e)
            {
                throw;
            }
            
            
            return _returnValues;
            
        }
    }
}
