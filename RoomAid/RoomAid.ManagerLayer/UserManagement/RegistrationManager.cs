﻿using RoomAid.DataAccessLayer;
using RoomAid.ServiceLayer;
using System;
using System.Configuration;
using System.Security.Cryptography;


namespace RoomAid.ManagerLayer
{
    public class RegistrationManager
    {
        private SHA256Cng algorithm = new SHA256Cng(); // SHA256 algorithm

        //For normal User
        /// <summary>
        /// Method RegisterUser() will check all user input and see if they are valid 
        /// <param name="email">The input email, should be passed from fortnend or controller, used as username</param>
        /// <param name="fname">User first name passed from frontend</param>
        /// <param name="lname">User last name passed from frontend</param>
        /// <param name="dob">User's date of birth passed from frontend</param>
        /// <param name="gender">User's gender passed from frontend</param>
        /// <param name="password">User's password passed from frontend</param>
        /// <returns>Iresult result the object that contains a message and if the check is true or false</returns>
        public IResult RegisterUser(RegistrationRequestDTO registrationRequest)
        {
            ValidationService rs = new ValidationService();
            string message = "";
            bool ifSuccess = false;

            string email = registrationRequest.Email;
            string fname = registrationRequest.Fname;
            string lname = registrationRequest.Lname;
            DateTime dob = registrationRequest.Dob;
            string gender = registrationRequest.Gender;
            string password = registrationRequest.Password;
            string repassword = registrationRequest.Repassword;


            IResult checkResult = rs.EmailValidation(email);

            message = message + checkResult.Message;
            ifSuccess = checkResult.IsSuccess;

            checkResult = rs.NameValidation(fname);
            message = message + checkResult.Message;
            ifSuccess = checkResult.IsSuccess;

            checkResult = rs.NameValidation(lname);
            message = message + checkResult.Message;
            ifSuccess = checkResult.IsSuccess;

            checkResult = rs.AgeValidation(dob, Int32.Parse(ConfigurationManager.AppSettings["ageRequired"]));
            message = message + checkResult.Message;
            ifSuccess = checkResult.IsSuccess;

            checkResult = rs.PasswordValidation(password);
            message = message + checkResult.Message;
            ifSuccess = checkResult.IsSuccess;

            checkResult = rs.PasswordUserNameValidation(password, email);
            message = message + checkResult.Message;
            ifSuccess = checkResult.IsSuccess;

            if (password != repassword)
            {
                message = message + "/n" + ConfigurationManager.AppSettings["passwordNotMatch"];
                ifSuccess = false;
            }

            if (ifSuccess)
            {
                // TODO: store email in separate mapping table?
                // Hash email and store hashed value
                Hasher hasher = new Hasher(algorithm);
                string hashedEmail = hasher.GenerateHash(email);
                User newUser = new User(hashedEmail, fname, lname, "Enable", dob, gender);

                // Generate salt and hash password
                HashObject hash = hasher.GenerateSaltedHash(password);
                string hashedPw = hash.HashedValue;
                string salt = hash.Salt;

                // Call the service to add user
                CreateAccountService ad = new CreateAccountService();
                checkResult = ad.CreateAccount(newUser);
                message = message + checkResult.Message;
                ifSuccess = checkResult.IsSuccess;

                // TODO: If success, call UpdateUser() function to add pw and salt into db
            }

            //log
            Logger.Log(message);
            return new CheckResult(message, ifSuccess);
        }

    }
}
