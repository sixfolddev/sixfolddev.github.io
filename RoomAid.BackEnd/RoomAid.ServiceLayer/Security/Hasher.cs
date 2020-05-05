using RoomAid.DataAccessLayer;
using System;
using System.Security.Cryptography;

namespace RoomAid.ServiceLayer
{
    public class Hasher
    {
        // Algorithm to be used for hashing
        private readonly HashAlgorithm _algorithm;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Hasher()
        {
            _algorithm = new SHA256Cng(); // default: SHA256 algorithm
        }

        /// <summary>
        /// Constructor that takes in a specified hash algorithm
        /// </summary>
        /// <param name="alg">Algorithm to be used for hashing</param>
        public Hasher(HashAlgorithm alg)
        {
            _algorithm = alg;
        }

        /// <summary>
        /// Generates a hash, given a value, using the assigned algorithm at declaration
        /// </summary>
        /// <param name="value">Value to be hashed</param>
        /// <returns>A hashed value in string format</returns>
        public string GenerateHash(string value)
        {
            byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes(value);
            byte[] hashBytes = _algorithm.ComputeHash(valueBytes);
            string hashString = BitConverter.ToString(hashBytes); // Returns hyphenated base 16 string
            hashString = hashString.Replace("-", "");

            return hashString;
        }

        /// <summary>
        /// Generates a unique salt and appends that salt to the given value to generate a salted hash
        /// </summary>
        /// <param name="value">Value to be hashed</param>
        /// <returns>A HashObject which contains the hashed value and the associated salt</returns>
        public HashObject GenerateSaltedHash(string value)
        {
            string salt = SaltGenerator.GenerateSalt();
            string saltedValue = value + salt;
            string hashString = GenerateHash(saltedValue);

            HashObject hash = new HashObject { HashedValue = hashString, Salt = salt };
            return hash;
        }

        /// <summary>
        /// Appends the given salt to the given value and generates a hash
        /// </summary>
        /// <param name="value">Value to be hashed</param>
        /// <param name="salt">Salt to be appended to a value</param>
        /// <returns>A hashed value in string format</returns>
        public string GenerateSaltedHash(string value, string salt)
        {
            string saltedValue = value + salt;
            return GenerateHash(saltedValue);
        }
    }
}