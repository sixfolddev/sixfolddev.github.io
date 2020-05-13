using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RoomAid.ServiceLayer
{
    public class JWTService
    {

        private readonly string _secretkey;
        private readonly int _sessiontimeout;
        private readonly Base64UrlConverter _encoder;

        // Claims
        private const string ISSUED_AT_TIME = "iat";
        private const string EXPIRATION_TIME = "exp";
        //public const string JWT_ID = "jti";
        private const string SUB = "sub";
        //public const string ADMIN = "admin";

        public JWTService()
        {
            _secretkey = ConfigurationManager.AppSettings["_secretkey"];
            _sessiontimeout = Int32.Parse(ConfigurationManager.AppSettings["sessiontimeout"]); // 20 minute session timeout
            _encoder = new Base64UrlConverter();
        }

        private string GenerateJWTHeader()
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("alg", "HS256"); // HMAC SHA256
            header.Add("typ", "JWT");
            string encodedHeader = _encoder.Encode(header);

            return encodedHeader;
        }

        private string GenerateJWTPayload(User user)
        {
            Dictionary<string, string> claims = new Dictionary<string, string>();
            claims.Add(ISSUED_AT_TIME, getTimeNowInSeconds().ToString());
            claims.Add(EXPIRATION_TIME, (getTimeNowInSeconds() + _sessiontimeout).ToString());
            //claims.Add(JWT_ID, Guid.NewGuid().ToString()); // TODO: Do not use GUID
            claims.Add(SUB, (user.SystemID).ToString()); // TODO: encrypt email
            //claims.Add(ADMIN, user.Admin.ToString());
            string encodedPayload = _encoder.Encode(claims);

            return encodedPayload;
        }

        public Int64 getTimeNowInSeconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        private string GenerateJWTSignature(string key, string header, string payload)
        {
            var sb = new StringBuilder(header);
            sb.Append('.');
            sb.Append(payload);
            byte[] headPayInBytes = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] keyInBytes = Encoding.UTF8.GetBytes(key);
            HMACSHA256 hash = new HMACSHA256(keyInBytes);
            byte[] signature = hash.ComputeHash(headPayInBytes);
            hash.Dispose();
            string encodedSignature = _encoder.CleanString(signature);

            return encodedSignature;
        }

        // Token = header.payload.signature
        public string GenerateJWT(User user)
        {
            string header = GenerateJWTHeader();
            string payload = GenerateJWTPayload(user);
            string signature = GenerateJWTSignature(_secretkey, header, payload);

            string token = $"{header}.{payload}.{signature}";
            
            return token;
        }
    }
}
