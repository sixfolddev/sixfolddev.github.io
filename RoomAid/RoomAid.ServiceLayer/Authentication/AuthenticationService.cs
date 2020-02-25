using System;
using System.Security.Cryptography;
using System.Text;

namespace RoomAid.ServiceLayer
{
    public class AuthenticationService
    {
        private string _userEmail;
        private string _password;
        private bool _authenticated;
        private Hasher hasher = new Hasher(new SHA256Cng());

        //CreateAccount object when user tries to log in
        public AuthenticationService(string email, string password)
        {
            //Get salt from database tied to input email
            //Call method to hash input password and salt
            //Call method to retrieve stored hash password
            //Compare generated password with stored password
            //Don't store into variables
            this._userEmail = email;
            this._password = password;
            _authenticated = false;
        }

        public bool Authenticate()
        {
            if (CompareHashes())
                _authenticated = true;
            else
                _authenticated = false;

            return _authenticated;
        }

        public bool CompareHashes()
        {
            // TODO: Get salt from db
            string salt = GetSalt(_userEmail);

            // Compare hashed passwords
            if (hasher.GenerateSaltedHash(_password, salt) == RetrieveHashFromDataStore())
            {
                return true;
            }

            return false;
        }

        /*public string GenerateHash(string userID, string password)
        {
            int iterations = 100000;
            //concatenate salt and input password to run through hashing

            var hash = new Rfc2898DeriveBytes(password, GetSalt(userID), 
                iterations, HashAlgorithmName.SHA256);
            var passwordToCheck = Encoding.Default.GetString(hash.GetBytes(32));

            return passwordToCheck;
        }
        */

        // QUERY TO FIND ASSOCIATED ACCOUNT, THEN GRAB HASH AND SALT

        public string RetrieveHashFromDataStore()
        {
            string storedHash;
            try
            {
                // Query to retrieve hash from datastore
                storedHash = "tempHash"; // temp hash
            }
            catch (Exception)
            {
                //If hashed pw cannot be retrieved, AuthenticationService will fail because
                //the comparison will fail
                storedHash = "";
            }

            return storedHash;
        }

        public string GetSalt(string userID)
        {
            try
            {
                // Query to retrieve salt from datastore
                return Encoding.ASCII.GetBytes("AE2012DEWE193241").ToString(); //test salt
            }
            catch (Exception)
            {
                //Catch error handling.
                //Returns error back to login screen if salt doesn't exist 
                //which means account doesn't exist.
                return Encoding.ASCII.GetBytes("").ToString();
            }
        }

        public bool GetAuthenticated()
        {
            return _authenticated;
        }
    }
}

