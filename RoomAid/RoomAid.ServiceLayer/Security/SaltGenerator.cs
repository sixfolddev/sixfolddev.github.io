using System;
using System.Security.Cryptography;

namespace RoomAid.ServiceLayer
{
    public class SaltGenerator
    {
        public static string GenerateSalt()
        {
            var saltbytes = new byte[32];

            var random = new RNGCryptoServiceProvider();
            random.GetBytes(saltbytes);

            return BitConverter.ToString(saltbytes);
        }
    }
}
