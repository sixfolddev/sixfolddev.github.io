using RoomAid.DataAccessLayer;
using System;
using System.Security.Cryptography;

namespace RoomAid.ServiceLayer
{
    public class Hasher
    {
        private readonly HashAlgorithm _algorithm;

        public Hasher()
        {
            _algorithm = new SHA256Cng(); // default: SHA256 algorithm
        }

        public Hasher(HashAlgorithm alg)
        {
            _algorithm = alg;
        }

        public string GenerateHash(string value)
        {
            byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes(value);
            byte[] hashBytes = _algorithm.ComputeHash(valueBytes);
            string hashString = BitConverter.ToString(hashBytes); // Returns hyphenated base 16 string
            hashString = hashString.Replace("-", "");

            return hashString;
        }

        public HashObject GenerateSaltedHash(string value)
        {
            string salt = SaltGenerator.GenerateSalt();
            string saltedValue = value + salt;
            string hashString = GenerateHash(saltedValue);

            HashObject hash = new HashObject { HashedValue = hashString, Salt = salt };
            return hash;
        }

        public string GenerateSaltedHash(string value, string salt)
        {
            string saltedValue = value + salt;
            return GenerateHash(saltedValue);
        }
    }
}