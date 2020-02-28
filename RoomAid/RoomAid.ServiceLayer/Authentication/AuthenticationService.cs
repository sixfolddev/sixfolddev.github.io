using System;
using System.Security.Cryptography;
using System.Text;

namespace RoomAid.ServiceLayer
{
    public class AuthenticationService
    {
        private readonly string _userEmail;
        private bool _authenticated;
        private readonly Hasher hasher = new Hasher(new SHA256Cng());

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

        public bool CompareHashes(string password)
        {
            string salt = GetSalt(_userEmail);
            if (hasher.GenerateSaltedHash(password, salt) == RetrieveDataStoreHash())
            {
                return true;
            }
            return false;
        }

        /*
        public string GenerateHash(string _userEmail, string _password)
        {
            int iterations = 100000;
            //concatenate salt and input _password to run through hashing

            var hash = new Rfc2898DeriveBytes(_password, GetSalt(_userEmail), 
                iterations, HashAlgorithmName.SHA256);
            var passwordToCheck = Encoding.Default.GetString(hash.GetBytes(32));

            return passwordToCheck;
        }*/

        public string RetrieveDataStoreHash()
        {
            string storedHash;
            try
            {
                //Retrieve hash connected to user ID from pw file
                storedHash = "tempHash";/*"f8qÈessKÉü`\u0002æça'\u0014éãPHê\u008d¥çE\u0005\u0004Kc²e";*/
            }
            catch (Exception)
            {
                //If hashed pw cannot be retrieved, AuthenticationService will fail because
                //the comparison will fail
                storedHash = "";
            }

            return storedHash;
        }

        public string GetSalt(string _userEmail)
        {
            try
            {
                //pull salt from pw file
                return "tempsalt"; // temp salt
            }
            catch (Exception)
            {
                //Catch error handling.
                //Returns error back to login screen if salt doesn't exist 
                //which means account doesn't exist.
                return "";
            }
        }
/*
        public bool GetAuthenticated()
        {
            return _authenticated;
        }*/
    }
}

