using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer.HouseholdSearch
{
    public class HouseholdSearchByCitySQL : IHouseholdSearchService
    {
        // User input
        private string _city;
        // Keyset
        private int _lastHID;
        // User filters
        private readonly List<String> _filters;
        // Number of results per page
        private readonly int _limit = 20;
        private string _db = "Data Source=DESKTOP-9ROILOO;Initial Catalog = Roomaid System Database; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        /// <summary>
        /// Keyset pagination requires the id of the last searched item, provided by the client
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
        public IResult Search()
        {

            using (SqlConnection conn = new SqlConnection(_db)) {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    "SELECT TOP " + this._limit + " * FROM HouseholdListings WHERE HID IN (SELECT HID FROM Households WHERE ZipCode IN(SELECT ZipCode FROM ZipLocations WHERE City = \'" + this._city + "\') AND IsAvailable = 1) AND HID > " + this._lastHID + " ORDER BY HID ASC;");
                command.Connection = conn;
                command.ExecuteNonQuery();
                conn.Close();
            }
            

            return new CheckResult("Search returned", true);
        }
    }
}
