
using System;
using RoomAid.DataAccessLayer;
using RoomAid.ServiceLayer;


namespace RoomAid.ManagerLayer.HouseHoldManagement
{
    public class HouseHoldManager
    {
        public IResult CreateNewHouseHold(HouseHoldCreationRequestDTO request)
        {
            User requester = request.Requester;
            string streetAddress = request.StreetAddress;
            string city = request.City;
            int zip = request.Zip;
            string suiteNumber = request.SuiteNumber;
            double rent = request.Rent;

            bool ifValid = true;
            string message = "";

            ValidationService vs = new ValidationService();

            //Check input length for streetAddress
            IResult result = vs.LengthValidation(streetAddress, 200, 1);

            if (!result.IsSuccess)
            {
                ifValid = false;
                message += result.Message;
            }

            //Check input length for city
            result = vs.LengthValidation(city, 200, 1);
            if (!result.IsSuccess)
            {
                ifValid = false;
                message +=  "\n"+result.Message;
            }

            //ZipCode for CA is from 90001 to 96162
            if (!vs.ZipValidation_CA(zip))
            {
                ifValid = false;
                message += "\nInvalid ZipCode! This ZipCode does not exist in CA";
            }

            if(!vs.DecimalValidation(rent, 2))
            {
                ifValid = false;
                message += "\nInvalid rent input! Should have no more than 2 decimal places";
            }
            if (ifValid)
            {
                IHouseHoldDAO dao = new SqlHouseHoldDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
                HouseHoldService hhs = new HouseHoldService(dao);

                //by defaut new household will be 'unavailable'
                HouseHold newHouseHold = new HouseHold(rent, streetAddress, zip, false);
                int hId = hhs.CreateHouseHold(newHouseHold);

                if (hId > 0)
                {
                    message += "\nHouseHold Created Successfully!";
                }
                else
                {
                    ifValid = false;
                    message += "\nFailed to create new HouseHold! HouseHold Already exist";
                }

            }

            return new CheckResult(message,ifValid);
        }

    }
}
