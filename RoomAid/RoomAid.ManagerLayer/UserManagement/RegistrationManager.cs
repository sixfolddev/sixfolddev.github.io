using RoomAid.DataAccessLayer;
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
            int sysID = -1;

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
                // Generate salt and hash password
                Hasher hasher = new Hasher(algorithm);
                HashObject hash = hasher.GenerateSaltedHash(password);
                string hashedPw = hash.HashedValue;
                string salt = hash.Salt;
                Account newAccount = new Account(email, hashedPw, salt);
                // Call the service to create Account
                IUpdateAccountDAO DAO = new UpdateAccountSqlDAO(ConfigurationManager.AppSettings["sqlConnectionAccount"]);
                ICreateAccountService cas = new SqlCreateAccountService(newAccount);
                checkResult = cas.Create();
                message = message + checkResult.Message;
                ifSuccess = checkResult.IsSuccess;
     
                // TODO: user the return ID to update User
                IUpdateAccountDAO updateDAO = new UpdateAccountSqlDAO(ConfigurationManager.AppSettings["sqlConnection"]);
                //UpdateAccountSqlService updateAccount = new UpdateAccountSqlService(newUser, updateDAO);
                //checkResult = updateAccount.Update();
                message = message + checkResult.Message;
                ifSuccess = checkResult.IsSuccess;
            }

            //log
            LogService.Log(message);
            return new CheckResult(message, ifSuccess);
        }

    }
}
