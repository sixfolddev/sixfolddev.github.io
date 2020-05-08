using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer.HouseHoldListing
{
    public interface IHHListingDAO
    {
        /// <summary>
        /// Inserts HouseholdListing Information recorded in data store in a default or provided HHListingModel.
        /// </summary>
        /// <param name="model">HHListingModel used as a basis</param>
        /// <returns></returns>
        int Insert(HHListingModel model);

        /// <summary>
        /// Updates HouseholdListing Information recorded in data store a default or provided HHListingModel.
        /// </summary>
        /// <param name="model">HHListingModel used as a basis</param>
        /// <returns></returns>
        int Update(HHListingModel model);

        /// <summary>
        /// Retrieves HouseholdListing Information of a household with the provided unique identifier.
        /// </summary>
        /// <param name="hid">Household's unique identifier</param>
        /// <returns></returns>
        HHListingModel Retrieve(int hid);

        /// <summary>
        /// Deletes HouseholdListing Information of a household with the provided unique identifier.
        /// </summary>
        /// <param name="hid">Household's unique identifier</param>
        /// <returns></returns>
        int Delete(int hid);
    }
}
