using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace RoomAid.DataAccessLayer.HouseHoldListing 
{
    public class HHListingDAO
    {
        private readonly string _dbConnectionString;

        HHListingDAO(string connection)
        {
            this._dbConnectionString = connection;
        }

        public int Insert(HHListingModel model)
        {
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction("HouseholdListing Creation");

                try
                {
                    var query = "INSERT into dbo.HouseholdListings(HID, HouseholdType, ListingDescription, Price, DatePosted) VALUES (@hid, @type, @desc, @price, @date)";
                    using(SqlCommand command = new SqlCommand(query, connection, transaction))
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
                            throw new Exception("Error in Household Listing Creation");
                        }
                        transaction.Commit();
                        return rowsChanged;
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }

        public int Update(HHListingModel model)
        {
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction("HouseholdListing Update");

                try
                { 
                    var query1 = "Update  dbo.HouseholdListings SET DatePosted = @date, HouseholdType = @hhtype, ListingDescription = @desc, Price = @price WHERE HID = @hid";
                    var query2 = "Update dbo.Households SET ZipCode = @zipcode, StreetAddress = @address, Availability = @availability, WHERE HID = @hid";


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
                            throw new Exception("Error in Household Listing Update");
                        }

                        transaction.Commit();
                        return rowsChanged;
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }

        public int Delete(int hid)
        {
            throw new NotImplementedException();
        }

        public int Retrieve(int hid)
        {
            throw new NotImplementedException();
        }
    }
}
