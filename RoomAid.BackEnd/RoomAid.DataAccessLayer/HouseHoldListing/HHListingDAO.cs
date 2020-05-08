using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace RoomAid.DataAccessLayer.HouseHoldListing
{
    public class HHListingDAO : IHHListingDAO
    {

        //TODO: Migrate queries to app.config file
        private readonly string _dbConnectionString;

        /// <summary>
        /// Public constructor for use by the HouseholdListingService.
        /// </summary>
        /// <param name="connection">database connection string to be used.</param>
        public HHListingDAO(string connection)
        {
            this._dbConnectionString = connection;
        }

        /// <summary>
        /// Inserts a record in the DAtabase with the information provided in the HHListingModel. Returns 1 for success, 0 for failure.
        /// </summary>
        /// <param name="model">Presumably a default HHListingModel used to insert a HHListing record upon household creation.</param>
        /// <returns></returns>
        public int Insert(HHListingModel model)
        {
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction("HouseholdListing Creation");

                try
                {
                    var query = "INSERT into dbo.HouseholdListings(HID, HouseholdType, ListingDescription, Price, DatePosted) VALUES (@hid, @type, @desc, @price, @date)";
                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@hid", model.HID);
                        command.Parameters.AddWithValue("@date", model.DatePosted);
                        command.Parameters.AddWithValue("@type", model.HouseholdType);
                        command.Parameters.AddWithValue("@desc", model.ListingDescription);
                        command.Parameters.AddWithValue("@price", model.Price);

                        var result = command.BeginExecuteNonQuery();

                        var rowsChanged = command.EndExecuteNonQuery(result);
                        if (rowsChanged != 1)
                        {
                            throw new Exception("Error encountered in Household Listing Creation");
                        }
                        transaction.Commit();
                        return rowsChanged;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    // throw e; Not sure if I should throw back an exception or the sql exception. regardless we will rollback and should send an appropriate value.
                    return 0;
                }
            }
        }

        /// <summary>
        /// Updates DatePosted, HouseholdType, ListingDescription, Price, ZipCode, StreetAddress, and Availability based on model. Returns 1 for success and 0 for failure.
        /// </summary>
        /// <param name="model">HouseholdListing Model provided as a basis to edit a preexisting record.</param>
        /// <returns></returns>
        public int Update(HHListingModel model)
        {
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction("HouseholdListing Update");

                try
                {
                    var query1 = "Update  dbo.HouseholdListings SET DatePosted = @date, HouseholdType = @hhtype, ListingDescription = @desc, Price = @price WHERE HID = @hid";
                    var query2 = "Update dbo.Households SET ZipCode = @zipcode, StreetAddress = @address, IsAvailable = @availability WHERE HID = @hid";


                    using (SqlCommand command = new SqlCommand(query1, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@hid", model.HID);
                        command.Parameters.AddWithValue("@date", model.DatePosted);
                        command.Parameters.AddWithValue("@availability", model.Availability);
                        command.Parameters.AddWithValue("@zipcode", model.ZipCode);
                        command.Parameters.AddWithValue("@address", model.StreetAddress);
                        command.Parameters.AddWithValue("@hhtype", model.HouseholdType);
                        command.Parameters.AddWithValue("@desc", model.ListingDescription);
                        command.Parameters.AddWithValue("@price", model.Price);

                        var result = command.BeginExecuteNonQuery();
                        var rowsChanged = command.EndExecuteNonQuery(result);

                        if (rowsChanged != 1)
                        {
                            throw new Exception("Error in Household Listing Update");
                        }

                        command.CommandText = query2;
                        result = command.BeginExecuteNonQuery();
                        rowsChanged = command.EndExecuteNonQuery(result);

                        if (rowsChanged != 1)
                        {
                            throw new Exception("Error encountered in Household Listing Update");
                        }

                        transaction.Commit();
                        return rowsChanged;
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return 0;
                    //throw e;
                }
            }
        }

        /// <summary>
        /// Deletes a single household listing record. Returns 1 for success, 0 for failure.
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        public int Delete(int hid)
        {
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction("HouseholdListing Delete");

                try
                {
                    var query = "Delete from dbo.HouseholdListings WHERE HID = @hid";

                    using (var command = new SqlCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@hid", hid);

                        var result = command.BeginExecuteNonQuery();

                        var rowsChanged = command.EndExecuteNonQuery(result);

                        if (rowsChanged != 1)
                        {
                            throw new Exception("Error encountered in Household Deletion");
                        }

                        transaction.Commit();
                        return rowsChanged;
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return 0;
                    //throw e;
                }
            }
        }

        /// <summary>
        /// Returns a single HHListingModel with the provided hid.
        /// Retrieves ZipCode Availability, DatePosted, ListingDescription, Price, Host First and Last Name.
        /// Note: Added retrieve HouseholdType despite not specified in householdListing viewing.
        /// </summary>
        /// <param name="hid">unique identifier of a household.</param>
        /// <returns></returns>
        public HHListingModel Retrieve(int hid)
        {
            HHListingModel model;
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction("HouseholdListing Retrieve");

                try
                {
                    var query = "SELECT ZipCode, IsAvailable from dbo.Households WHERE HID = @hid;" +
                                "SELECT HouseHoldType, ListingDescription, Price, DatePosted from dbo.HouseholdListings WHERE HID = @hid;" +
                                "SELECT FirstName, LastName from Residents INNER JOIN Users on Residents.SysID = Users.SysID WHERE HID = @hid;";

                    using (var command = new SqlCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@hid", hid);

                        var result = command.BeginExecuteReader();

                        var reader = command.EndExecuteReader(result);

                        #region Reads 1st Result of the batch query specified in var query.
                        using (reader)
                        {
                            if (!reader.Read())
                            {
                                throw new Exception("Error encountered in HouseholdListing Retrieval");
                            }
                            var zip = (string)reader["ZipCode"];
                            var availability = reader.GetSqlBoolean(reader.GetOrdinal("IsAvailable")); //Previous error (SqlBoolean Cast unacceptable)

                            //If there are still rows in the query, then the program has retrieved more than 1 householdListing with specified HID
                            if (reader.Read())
                            {
                                throw new Exception("Error encountered in HouseholdListing Retrieval");
                            }

                            #endregion


                            #region Reads 2nd result of the batch query specified in var query.
                            reader.NextResult();
                            if (!reader.Read())//Read the first result of the batch query specified in var query.
                            {
                                throw new Exception("Error encountered in HouseholdListing Retrieval");
                            }
                            var datePosted = (DateTime)reader["DatePosted"];
                            var householdType = (string)reader["HouseHoldType"];
                            var description = (string)reader["ListingDescription"];
                            var price = (decimal)reader["Price"];
                            //If there are still rows in the query, then the program has retrieved more than 1 householdListing with specified HID
                            if (reader.Read())
                            {
                                throw new Exception("Error encountered in HouseholdListing Retrieval");
                            }
                            #endregion


                            #region Reads the third result set of the batch query specified in var query.
                            reader.NextResult();
                            var hostName = "Not Available";
                            if (reader.Read())//Read the first result of the batch query specified in var query.
                            {
                                var firstName = (string)reader["FirstName"];
                                var lastName = (string)reader["LastName"];
                                hostName = firstName + lastName;
                            }
                            //If there are still rows in the query, then the program has retrieved more than 1 householdListing with specified HID
                            if (reader.Read())
                            {
                                throw new Exception("Error encountered in HouseholdListing Retrieval");
                            }
                            #endregion

                            model = new HHListingModel(hid, datePosted, availability, hostName: hostName, zipCode: zip, householdType: householdType, listingDescription: description, price: price);

                        }
                        transaction.Commit();
                        return model;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    model = new HHListingModel();
                    return model;
                }
            }
        }
    }
}