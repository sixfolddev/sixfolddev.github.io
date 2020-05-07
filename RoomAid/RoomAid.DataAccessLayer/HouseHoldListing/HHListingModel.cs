using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;

namespace RoomAid.DataAccessLayer.HouseHoldListing
{
    public class HHListingModel
    {
        /// <summary>
        /// Default values for optional parameters when constructing an HHListingModel.
        /// </summary>
        const string _defaultHostName = "Not Available";
        const string _defaultZipCode = "00000";
        const string _defaultStreetAddress = "Not Available";
        private SqlBoolean _defaultAvailability = SqlBoolean.False;
        const string _defaultHouseholdType = "Not Specified";
        const string _defaultListingDescription = "";
        const decimal _defaultPrice = decimal.Zero;

        public const int maxHostName = 200;
        public const int maxZipCode = 5;
        public const int maxStreetAddress = 200;
        public const int maxHouseholdType = 30;
        public const int maxListingDescription = 200;

        /// <summary>
        /// Getters for HHListingModel attributes.
        /// </summary>
        public int HID { get; }
        public SqlBoolean Availability { get; }
        public DateTime DatePosted { get; }

        public string HostName { get;}
        public string ZipCode { get; }
        public string StreetAddress { get; }
        public string HouseholdType { get; }
        public string ListingDescription { get; }
        public decimal Price { get; }

        /// <summary>
        /// Default constructor for a HHListingModel. Used when retreiving a non existn HHListingModel
        /// </summary>
        public HHListingModel()
        {
            HID = 0;
            DatePosted = DateTime.UtcNow;
            Availability = SqlBoolean.False;
            HostName = _defaultHostName;
            ZipCode = _defaultZipCode;
            StreetAddress = _defaultStreetAddress;
            HouseholdType = _defaultHouseholdType;
            ListingDescription = _defaultListingDescription;
            Price = _defaultPrice;

        }

        /// <summary>
        /// Used for Insert of a HHListing with default information
        /// </summary>
        /// <param name="hid">Household ID</param>
        public HHListingModel(int hid)
        {
            HID = hid;
            DatePosted = DateTime.UtcNow;
            Availability = SqlBoolean.False;
            HostName = _defaultHostName;
            ZipCode = _defaultZipCode;
            StreetAddress = _defaultStreetAddress;
            HouseholdType = _defaultHouseholdType;
            ListingDescription = _defaultListingDescription;
            Price = _defaultPrice;

        }

        /// <summary>
        /// Custom Constructor for HHListingModel
        /// </summary>
        /// <param name="hid">HouseholdID: Mandatory</param>
        /// <param name="datePosted">Date of Posting: Mandatory for Insert, Update with values of DateTime.UtcNow, Retrieve with database value.</param>
        /// <param name="availability">Expression of Availability: Mandatory. Set by User when editing. Set by db when retrieving</param>
        /// <param name="hostName">Concatenation of Host's First and Last Name. Max 200 characters.</param>
        /// <param name="zipCode">Household's 5 digit Zip Code</param>
        /// <param name="streetAddress">Household's Street Address. Max 200 characters. </param>
        /// <param name="householdType">HouseholdListing's Type. Max 30 characters.</param>
        /// <param name="listingDescription">Description of HouseholdListing. Max 2000 characters</param>
        /// <param name="price">"Listings suggested price. Decimal (19,4) </param>
        public HHListingModel(int hid, DateTime datePosted, SqlBoolean availability, string hostName = _defaultHostName, 
            string zipCode = _defaultZipCode, string streetAddress = _defaultStreetAddress, 
            string householdType = _defaultHouseholdType, string listingDescription = _defaultListingDescription, decimal price = _defaultPrice )
        {

        }

        
    }
}
