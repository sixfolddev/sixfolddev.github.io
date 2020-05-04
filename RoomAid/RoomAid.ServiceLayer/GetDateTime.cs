using System;

namespace RoomAid.ServiceLayer
{
    public static class GetDateTime
    {
        public static DateTime GetUTCNow()
        {
            return DateTime.UtcNow;
        }

        public static DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}
