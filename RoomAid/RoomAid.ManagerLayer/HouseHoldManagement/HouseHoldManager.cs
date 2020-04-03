﻿
using System;
using System.Configuration;
using System.Data.SqlClient;
using RoomAid.DataAccessLayer;
using RoomAid.ServiceLayer;


namespace RoomAid.ManagerLayer.HouseHoldManagement
{
    public class HouseHoldManager
    {
        private IHouseHoldDAO dao = new SqlHouseHoldDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
    
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
            ifValid = IfUserExist(requester.SystemID);

            if (ifValid)
            {
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
                    message += "\n" + result.Message;
                }

                //ZipCode for CA is from 90001 to 96162
                if (!vs.ZipValidation_CA(zip))
                {
                    ifValid = false;
                    message += "\nInvalid ZipCode! This ZipCode does not exist in CA";
                }

                if (!vs.DecimalValidation(rent, 2))
                {
                    ifValid = false;
                    message += "\nInvalid rent input! Should have no more than 2 decimal places";
                }
            }
            else
            {
                ifValid = false;
                message += "\nThe Requester's ID doese not exist";
            }
            
            if (ifValid)
            {
                HouseHoldService hhs = new HouseHoldService(dao);
                streetAddress = streetAddress + ", " + city + ", " + suiteNumber;
                //by defaut new household will be 'unavailable'
                HouseHold newHouseHold = new HouseHold(rent, streetAddress, zip, false);
                int hId = hhs.CreateHouseHold(newHouseHold);

                if (hId > 0)
                {
                    message += "\nHouseHold Created Successfully!";
                    IResult createEmptyHouseHoldListing = hhs.CreateHouseHoldListing(hId);

                    ifValid = createEmptyHouseHoldListing.IsSuccess;
                    message += createEmptyHouseHoldListing.Message;
                }
                else
                {
                    ifValid = false;
                    message += "\nFailed to create new HouseHold! HouseHold Already exist";
                }

            }

            return new CheckResult(message,ifValid);
        }
        //Check if zip exist in the ZipLocation table
        private bool IfUserExist(int sID)
        {
            SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["querySelectSysID"]);
            command.Parameters.AddWithValue("@sysID", sID);
            if (dao.Retrive(command) > 0)
                return true;
            else
                return false;
        }
    }
}
