using RoomAid.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    public class HouseHoldService
    {
        private readonly IHouseHoldDAO dao;

        public HouseHoldService(IHouseHoldDAO dao)
        {
            this.dao = dao;
        }
        public int CreateHouseHold(HouseHold newHouseHold)
        {
            bool ifExist = IfHouseHoldExist(newHouseHold.StreetAddress, newHouseHold.Zip);
            bool ifZipValid = IfZipExist(newHouseHold.Zip);
            if (!ifExist&& ifZipValid)
            {
                SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["queryCreateHouseHold"]);
                command.Parameters.AddWithValue("@rent", newHouseHold.Rent);
                command.Parameters.AddWithValue("@streetAddress", newHouseHold.StreetAddress);
                command.Parameters.AddWithValue("@zipCode", newHouseHold.Zip);
                command.Parameters.AddWithValue("@isAvailable", newHouseHold.IsAvailable);
                return dao.Retrive(command);
            }
            else
                return 0;
        }

        //After CreatHouseHold success, this method will be called to create an empty houseHoldlisting
        public IResult CreateHouseHoldListing(int hID)
        {
            string message = "";
            bool result = false;
            if (IfHIDExist(hID))
            {
                SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["queryCreateHouseHoldListing"]);
                command.Parameters.AddWithValue("@hId", hID);
                command.Parameters.AddWithValue("@price", 0.00);
                if (dao.Insert(command) > 0)
                {
                    message = "Create HouseHoldListing successed!";
                    result = true;
                }        
                else
                    message = "Create HouseHoldListing failed!"; 
            }
            else
            {
                message = "HID does not exist!";
            }

            return new CheckResult(message, result);
          
        }

        //should check if the address is already used for new household 
        private bool IfHouseHoldExist(string streetAddress, int zip)
        {
            SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["querySelectHouseHold"]);
            command.Parameters.AddWithValue("@streetAddress", streetAddress);
            command.Parameters.AddWithValue("@zipCode", zip);
            if (dao.Retrive(command) > 0)            
                return true;           
            else
                return false;
        }

        //Check if zip exist in the ZipLocation table
        private bool IfZipExist( int zip)
        {
            SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["querySelectZip"]);
            command.Parameters.AddWithValue("@zipCode", zip);
            if (dao.Retrive(command) > 0)
                return true;
            else
                return false;
        }

        //Check if zip exist in the ZipLocation table
        private bool IfHIDExist(int hID)
        {
            SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["querySelectHID"]);
            command.Parameters.AddWithValue("@hid", hID);
            if (dao.Retrive(command) > 0)
                return true;
            else
                return false;
        }

        public int UpdateHouseHold(int hId)
        {
            return 0;
        }

        public bool DeleteHouseHold(int hId)
        {
            SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["queryDeleteHouseHold"]);
            command.Parameters.AddWithValue("@HID", hId);
            if (dao.Insert(command) > 0)
                return true;
            else
                return false;
        }

        public bool DeleteHouseHoldListing(int hId)
        {
            SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["queryDeleteHouseHoldListing"]);
            command.Parameters.AddWithValue("@HID", hId);
            if (dao.Insert(command) > 0)
                return true;
            else
                return false;
        }

    }
}
