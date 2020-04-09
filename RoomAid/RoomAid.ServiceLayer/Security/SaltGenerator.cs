using System;
using System.Security.Cryptography;

namespace RoomAid.ServiceLayer
{
    public class SaltGenerator
    {
        public static string GenerateSalt()
        {
            var saltbytes = new byte[32];

            using (var random = new RNGCryptoServiceProvider()) // Does indirect disposal of RNGCryptoService
            {
                random.GetBytes(saltbytes);
            }

            var salt = BitConverter.ToString(saltbytes); // Returns hyphenated base 16 string
            salt = salt.Replace("-", "");

            return salt;
        }
    }
}
