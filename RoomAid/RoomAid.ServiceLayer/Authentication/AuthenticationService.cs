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
        
        //Create object when user tries to log in
        public AuthenticationService(string email, string _password)
        {
            //Get salt from database tied to input account ID
            //Call method to hash input _password and salt
            //Call method to retrieve stored hash _password
            //Compare generated _password with stored _password
            //Don't store into variables
            this._userEmail = email;
            this._password = _password;
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
            string salt = GetSalt(_userEmail);

            if (hasher.GenerateSaltedHash(_password, salt) == RetrieveDataStoreHash())
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
                storedHash = "f8qÈessKÉü`\u0002æça'\u0014éãPHê\u008d¥çE\u0005\u0004Kc²e";
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

