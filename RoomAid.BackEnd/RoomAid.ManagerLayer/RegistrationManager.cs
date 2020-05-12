using RoomAid.DataAccessLayer;
using RoomAid.DataAccessLayerLayer;
using RoomAid.ServiceLayer;
using System;
using System.Security.Cryptography;


namespace RoomAid.ManagerLayer
{
    public class RegistrationManager
    {
        private readonly SHA256Cng algorithm = new SHA256Cng(); // SHA256 algorithm
        private readonly RegistrationRequestDTO registrationRequest;

        /// <summary>
        /// Service that finish registration and create account for new user
        /// </summary>
        /// <param name="RegistrationRequestDTO dto"></param>
        public RegistrationManager(RegistrationRequestDTO dto)
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
            string fname = registrationRequest.Firstname;
            string lname = registrationRequest.Lastname;
            DateTime dob = registrationRequest.Dob;
            string password = registrationRequest.Password;

            // Generate salt and hash password
            Hasher hasher = new Hasher(algorithm);
            HashObject hash = hasher.GenerateSaltedHash(password);
            string hashedPw = hash.HashedValue;
            string salt = hash.Salt;
            Account newAccount = new Account(email, hashedPw, salt);

            ICreateAccountDAO newAccountDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User));
            ICreateAccountDAO newMappingDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
            IMapperDAO mapperDAO = new SqlMapperDAO(Environment.GetEnvironmentVariable("sqlConnectionMapping", EnvironmentVariableTarget.User));
            ICreateAccountDAO newUserDAO = new SqlCreateAccountDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));

            CreateAccountDAOs daos = new CreateAccountDAOs(newAccountDAO, newMappingDAO, newUserDAO,mapperDAO);
            ICreateAccountService cas = new SqlCreateAccountService(newAccount, daos);
            IResult checkResult = cas.Create();
            message = message + checkResult.Message;
            bool  ifSuccess = checkResult.IsSuccess;

            if (ifSuccess)
            {
                int sysID = mapperDAO.GetSysID(email);
                if (sysID!=-1)
                {
                    ISqlDAO DAO = new SqlDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
                    User newUser = new User(sysID, email, fname, lname, "Enable", dob, null);
                    UpdateAccountSqlService updateAccount = new UpdateAccountSqlService(newUser, DAO);
                    checkResult = updateAccount.Update();
                    message = message + checkResult.Message;
                    ifSuccess = checkResult.IsSuccess;
                }
                else
                {
                    ifSuccess = false;
                    message = message + "failed to Retrieve sysID";
                }
               
            }

            return new CheckResult(message, ifSuccess);
        }
    }
}
