using RoomAid.DataAccessLayer;
using System;
using System.Security.Cryptography;

namespace RoomAid.ServiceLayer
{
    public class Hasher
    {
        private HashAlgorithm _algorithm;
        public Hasher(HashAlgorithm alg)
        {
            _algorithm = alg;
        }

        public HashDAO GenerateHash(string value)
        {
            string salt = SaltGenerator.GenerateSalt();
            string saltedValue = value + salt;
            byte[] saltedValueBytes = System.Text.Encoding.UTF8.GetBytes(saltedValue);

            byte[] hashBytes = _algorithm.ComputeHash(saltedValueBytes);
            string hashString = BitConverter.ToString(hashBytes);

            HashDAO hash = new HashDAO { HashedValue = hashString, Salt = salt };
            return hash;
        }
    }
}
