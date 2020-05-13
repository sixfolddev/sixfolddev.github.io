using MongoDB.Bson.IO;
using RoomAid.DataAccessLayer;
using RoomAid.DataAccessLayer.UserManagement;
using RoomAid.ServiceLayer.UserManagement;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace RoomAid.ServiceLayer
{
    public class AuthenticationService
    {
        public string _userEmail { get; set; }
        private bool _authenticated;
        private readonly Hasher hasher = new Hasher(new SHA256Cng());
        private readonly GetUserDao _dao;

        public AuthenticationService(GetUserDao dao)
        {
            _dao = dao;
            _authenticated = false;
        }

        public AuthenticationService(string email)
        {
            //Get salt from database tied to input account ID
            //Call method to hash input _password and salt
            //Call method to retrieve stored hash _password
            //Compare generated _password with stored _password
            //Don't store into variables
            this._userEmail = email;
            _authenticated = false;
        }

        public bool Authenticate(string password)
        {
            if (CompareHashes(password))
                _authenticated = true;
            else
                _authenticated = false;

            return _authenticated;
        }

        public User FindUser(LoginAttemptModel model)
        {
            var email = model.Email;
            var dictionary = _dao.RetrieveUser(email);

            return new User(int.Parse(dictionary["SystemID"]), email, dictionary["FirstName"], dictionary["LastName"], dictionary["AccountStatus"], DateTime.Parse(dictionary["DateOfBirth"]), dictionary["Gender"]);


        }

        public bool CompareHashes(string password)
        {
            Dictionary<string, string> storeAccountInfo = RetrievePasswordAndSalt();
            string storedHash = storeAccountInfo["Hash"];
            string storedSalt = storeAccountInfo["Salt"];

            if (hasher.GenerateSaltedHash(password, storedSalt) == storedHash)
            {
                return true;
            }
            return false;
        }

        public Dictionary<string, string> RetrievePasswordAndSalt()
        {
            var accountInfo = new Dictionary<string, string>();
            try
            {
                var dao = new AccountDAO(Environment.GetEnvironmentVariable("sqlConnectionAccount", EnvironmentVariableTarget.User));
                var command = new SqlCommand("SELECT HashPassword, Salt FROM dbo.Accounts WHERE UserEmail = @email");
                command.Parameters.AddWithValue("@email", _userEmail);
                accountInfo = dao.RetrieveAccountInfo(command);
            }
            catch (Exception)
            {
                //If hashed pw cannot be retrieved, AuthenticationService will fail because
                //the comparison will fail
            }
            return accountInfo;
        }
    }
}

