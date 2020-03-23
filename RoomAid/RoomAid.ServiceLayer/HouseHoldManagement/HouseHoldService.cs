using RoomAid.DataAccessLayer.HouseHoldManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer.HouseHoldManagement
{
    public class HouseHoldService
    {
        private readonly IHouseHoldDAO dao;

        public HouseHoldService(IHouseHoldDAO dao)
        {
            this.dao = dao;
        }
        public int CreateHouseHold(HouseHoldCreationRequestDTO request)
        {
            bool ifExist = IfHouseHoldExist(request.StreetAddress, request.Zip);

            if (!ifExist)
            {
                SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["queryCreateHouseHold"]);
                command.Parameters.AddWithValue("@rent", request.Rent);
                command.Parameters.AddWithValue("@streetAddress", request.StreetAddress);
                command.Parameters.AddWithValue("@zipCode", request.Zip);
                command.Parameters.AddWithValue("@isAvailable", request.IsAvailable);
            }
            return 0;
        }

        //should check if the address is already used for new household 
        private bool IfHouseHoldExist(string streetAddress, int zip)
        {
            SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["querySelectHouseHold"]);
            command.Parameters.AddWithValue("@streetAddress", streetAddress);
            command.Parameters.AddWithValue("@zipCode", zip);
            if (dao.RunQuery(command) > 0)            
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
            if (dao.RunQuery(command) > 0)
                return true;
            else
                return false;
        }

        public IResult CreateResident(int sysId, int hId)
        {
            return null;
        }

        public IResult UpdateResident(int sysId, int hId)
        {
            return null;
        }

        public IResult DeleteResident(int sysId, int hId)
        {
            return null;
        }
    }
}
