using RoomAid.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    public class RegistrationService
    {
        private readonly SHA256Cng algorithm = new SHA256Cng(); // SHA256 algorithm
        private readonly RegistrationRequestDTO registrationRequest;

        /// <summary>
        /// Service that finish registration and create account for new user
        /// </summary>
        /// <param name="RegistrationRequestDTO dto"></param>
        public RegistrationService(RegistrationRequestDTO dto)
        {
            registrationRequest = dto;
        }

        /// <summary>
        /// Complete Registration for the user
        /// </summary>
        /// <returns>IResult the result of whole registration process</returns>
        public IResult RegisterUser()
        {
            string message = "";

            string email = registrationRequest.Email;
            string fname = registrationRequest.Fname;
            string lname = registrationRequest.Lname;
            DateTime dob = registrationRequest.Dob;
            string gender = registrationRequest.Gender;
            string password = registrationRequest.Password;

            // Generate salt and hash password
            Hasher hasher = new Hasher(algorithm);
            HashObject hash = hasher.GenerateSaltedHash(password);
            string hashedPw = hash.HashedValue;
            string salt = hash.Salt;
            Account newAccount = new Account(email, hashedPw, salt);

            ICreateAccountService cas = new SqlCreateAccountService(newAccount);
            IResult checkResult = cas.Create();
            message = message + checkResult.Message;
            bool  ifSuccess = checkResult.IsSuccess;

            if (ifSuccess)
            {
                IMapperDAO mapperDAO = new SqlMapperDAO(ConfigurationManager.AppSettings["sqlConnectionMapping"]);
                int sysID = mapperDAO.GetSysID(email);
                if (sysID!=-1)
                {
                    IUpdateAccountDAO DAO = new UpdateAccountSqlDAO(ConfigurationManager.AppSettings["sqlConnectionSystem"]);
                    User newUser = new User(sysID, email, fname, lname, "Enable", dob, gender);
                    UpdateAccountSqlService updateAccount = new UpdateAccountSqlService(newUser, DAO);
                    checkResult = updateAccount.Update();
                    message = message + checkResult.Message;
                    ifSuccess = checkResult.IsSuccess;
                }
                else
                {
                    ifSuccess = false;
                    message = message + "failed to retrive sysID";
                }
               
            }

            return new CheckResult(message, ifSuccess);
        }
    }
}
