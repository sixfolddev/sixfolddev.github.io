using RoomAid.DataAccessLayer;
using RoomAid.ServiceLayer;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using RoomAid.ServiceLayer;
using RoomAid.ServiceLayer.Registration;
using RoomAid.ServiceLayer.UserManagement;

namespace RoomAid.ManagerLayer
{
    public class RegistrationManager
    {
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

                checkResult = rs.NameCheck(fname);
                message = message + checkResult.Message;
                ifSuccess = checkResult.IsSuccess;

                checkResult = rs.NameCheck(lname);
                message = message + checkResult.Message;
                ifSuccess = checkResult.IsSuccess;

                checkResult = rs.AgeValidation(dob);
                message = message + checkResult.Message;
                ifSuccess = checkResult.IsSuccess;


                checkResult = rs.PasswordValidation(password);
                message = message + checkResult.Message;
                ifSuccess = checkResult.IsSuccess;

                checkResult = rs.PasswordUserNameCheck(password, email);
                message = message + checkResult.Message;
                ifSuccess = checkResult.IsSuccess;

                if (password != repassword)
                {
                    message = message + "/n" + ConfigurationManager.AppSettings["passwordNotMatch"];
                    ifSuccess = false;
                }

                if (ifSuccess)
                {
                    User newUser = new User(email, fname, lname, "Enable", dob, gender);

                    Hasher hasher = new Hasher(new SHA256Cng()); // SHA256 algorithm
                    HashDAO hash = hasher.GenerateHash(password);
                    AddUserService ad = new AddUserService();

                    checkResult = ad.AddUser(newUser, hash.HashedValue, hash.Salt);
                    //TODO: Hash the password
                    //TODO: Store the salt
                    //TODO: Call the service to add user
                    CreateAccountService ad = new CreateAccountService();
                    checkResult = ad.CreateAccount(newUser);	
                    message = message + checkResult.Message;
                    ifSuccess = checkResult.IsSuccess;
                }

            //log
            Logger.Log(message);
            return new CheckResult(message, ifSuccess);
        }

    }
}
