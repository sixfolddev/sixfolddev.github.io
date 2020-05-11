using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer.HouseHoldManagement
{
    // TODO: Change in Requirements: Household Profiles will now have a date posted. the offset filter should be changed to order by date and queries need to be updated to include date

    public class HouseholdSearchDAO : IHouseholdSearchDAO
    {
        // BRD: Required limit on results per query
        private readonly int _resultLimit = 20;
        private readonly string _SQLConnectionString;
        private readonly ICollection<HouseholdSearchDTO> _listing = new Collection<HouseholdSearchDTO>();
        private readonly ICollection<string> _cities = new Collection<string>();
        public HouseholdSearchDAO(string connectionString)
        {
            _SQLConnectionString = connectionString;
        }
        /// <summary>
        /// The search method establishes a connection to the sql server database and determines which filter to apply based on the input from the user.
        /// All parameters passed will be checked via the frontend. The following are the ranges for each:
        ///     - cityName: Selected from the autocomplete list of cities from the database
        ///     - page: provided by the frontend when a user searches
        ///     - minPrice: default value of 0 and maximum value of 10,000 set by user
        ///     - maxPrice: default value of 0 and maximum value of 10,000 set by user
        ///     - householdType: selected from a drop down menu by the user
        /// Data is collected in batches of 20(requirement of BRD) via offset pagination and assigned into a data transfer object. A collection of these DTOs
        /// are returned.
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
                using (SqlConnection connection = new SqlConnection(_SQLConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd;


                    // HACK:System.InvalidOperationException: ExecuteReader: CommandText property has not been initialized when using appconfig
                    // HACK: How to include 'IS NOT NULL' through Parameters.AddWithValue to account for no type filter being selected
                    if (householdType.Equals("none"))
                    {
                        cmd = new SqlCommand
                        ("SELECT HouseholdType, ListingDescription, Price FROM HouseholdListings INNER JOIN Households INNER JOIN ZipLocations ON Households.ZipCode = ZipLocations.ZipCode ON Households.HID = HouseholdListings.HID WHERE HouseholdListings.HouseHoldType IS NOT NULL AND HouseholdListings.Price BETWEEN @minPrice AND @maxPrice AND Households.IsAvailable = 1 AND ZipLocations.City = @cityName ORDER BY Households.HID OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY");
                        cmd.Parameters.AddWithValue($"@{nameof(cityName)}", cityName);
                        cmd.Parameters.AddWithValue($"@minPrice", minPrice);
                        cmd.Parameters.AddWithValue($"@maxPrice", maxPrice);
                        cmd.Parameters.AddWithValue("@offset", (page - 1) * _resultLimit);
                        cmd.Parameters.AddWithValue("@limit", _resultLimit);
                    }
                    else
                    {
                        cmd = new SqlCommand("SELECT HouseholdType, ListingDescription, Price FROM HouseholdListings INNER JOIN Households INNER JOIN ZipLocations ON Households.ZipCode = ZipLocations.ZipCode ON Households.HID = HouseholdListings.HID WHERE HouseholdListings.HouseHoldType = COALESCE(@householdType, 'ALL') AND HouseholdListings.Price BETWEEN @minPrice AND @maxPrice AND Households.IsAvailable = 1 AND ZipLocations.City = @cityName ORDER BY Households.HID OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY");
                        cmd.Parameters.AddWithValue("@cityName", cityName);
                        cmd.Parameters.AddWithValue("@householdType", householdType);
                        cmd.Parameters.AddWithValue("@minPrice", minPrice);
                        cmd.Parameters.AddWithValue("@maxPrice", maxPrice);
                        cmd.Parameters.AddWithValue("@offset", (page - 1) * _resultLimit);
                        cmd.Parameters.AddWithValue("@limit", _resultLimit);
                    }

                    cmd.Connection = connection;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            HouseholdSearchDTO householdData = new HouseholdSearchDTO();
                            householdData.HouseholdType = reader["HouseholdType"].ToString();
                            householdData.ListingDescription = reader["ListingDescription"].ToString();
                            householdData.Price = (reader["Price"].ToString());
                            _listing.Add(householdData);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return _listing;
        }
        /// <summary>
        /// Method used for generating the total number of results expected from the query. This number will be used to determine the total
        /// number of pages that will be available to the user during a search.
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="page"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="householdType"></param>
        /// <returns></returns>
        public int GetTotalResultCountForQuery(string cityName, int minPrice, int maxPrice, string householdType)
        {
            var resultCount = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(_SQLConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd;
                    // HACK:System.InvalidOperationException: ExecuteReader: CommandText property has not been initialized when using appconfig
                    // HACK: How to include 'IS NOT NULL' through Parameters.AddWithValue to account for no type filter being selected
                    if (householdType.Equals("none"))
                    {
                        cmd = new SqlCommand
                        ("SELECT COUNT(HouseholdListings.HID) FROM HouseholdListings INNER JOIN Households INNER JOIN ZipLocations ON Households.ZipCode = ZipLocations.ZipCode ON Households.HID = HouseholdListings.HID WHERE HouseholdListings.HouseHoldType IS NOT NULL AND HouseholdListings.Price BETWEEN @minPrice AND @maxPrice AND Households.IsAvailable = 1 AND ZipLocations.City = @cityName");
                        cmd.Parameters.AddWithValue("@cityName", cityName);
                        cmd.Parameters.AddWithValue("@minPrice", minPrice);
                        cmd.Parameters.AddWithValue("@maxPrice", maxPrice);
                    }
                    else
                    {
                        cmd = new SqlCommand("SELECT COUNT(HouseholdListings.HID) FROM HouseholdListings INNER JOIN Households INNER JOIN ZipLocations ON Households.ZipCode = ZipLocations.ZipCode ON Households.HID = HouseholdListings.HID WHERE HouseholdListings.HouseHoldType = @householdType AND HouseholdListings.Price BETWEEN @minPrice AND @maxPrice AND Households.IsAvailable = 1 AND ZipLocations.City = @cityName");
                        cmd.Parameters.AddWithValue("@cityName", cityName);
                        cmd.Parameters.AddWithValue("@householdType", householdType);
                        cmd.Parameters.AddWithValue("@minPrice", minPrice);
                        cmd.Parameters.AddWithValue("@maxPrice", maxPrice);
                    }

                    cmd.Connection = connection;
                    resultCount = (Int32)cmd.ExecuteScalar();
                    connection.Close();
                }
                return resultCount;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // TODO: Add functionality for returning city names for autocomplete frontend feature
        public ICollection<string> GetAutocompleteCities()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_SQLConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT DISTINCT City FROM ZipLocations");
                    cmd.Connection = connection;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _cities.Add(reader[0].ToString());
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return _cities;
        }
    }
}
