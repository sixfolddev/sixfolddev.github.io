using System;
using System.Web;
using System.Data.SqlClient;

namespace RoomAid.ServiceLayer
{
    /// <summary>
    /// Service that takes different exceptions and determines the threat based on the type and severity
    /// Returns a value Level, which is an enumerated List within the namespace RoomAid.ServiceLayer.Logging
    /// </summary>
    public partial class ErrorThreatService
    {
        public LogLevels.Levels GetThreatLevel(Exception exception)
        {
            return LogLevels.Levels.Warning;
        }
        /// <summary>
        /// test for throwing of timeoutException
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public LogLevels.Levels GetThreatLevel(TimeoutException exception)
        {
            return LogLevels.Levels.Error;
        }

        /// <summary>
        /// GetThreatLevel of SqlException based off of .Class property, which contains severity
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>LogLevels.Levels</returns>
        public LogLevels.Levels GetThreatLevel(SqlException exception)
        {
            if (exception.Class <= 10)
            {
                return LogLevels.Levels.Info;
            }
            else if (exception.Class <= 16)
            {
                return LogLevels.Levels.Warning;
            }
            else if (exception.Class <= 19)
            {
                return LogLevels.Levels.Error;
            }
            else
            {
                return LogLevels.Levels.Fatal;
            }
        }
        /// <summary>
        /// GetThreatLevel of UnauthorizedAccessException 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>LogLevels.Levels</returns>
        public LogLevels.Levels GetThreatLevel(UnauthorizedAccessException exception)
        {
            return LogLevels.Levels.Warning;  

        }
        /// <summary>
        /// GetThreatLevel of HttpUnhandledException
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>LogLevels.Levels</returns>
        public LogLevels.Levels GetThreatLevel(HttpUnhandledException exception)
        {
            return LogLevels.Levels.Warning;
        }
        

        public LogLevels.Levels GetThreatLevel(CustomException exception)
        {
            return exception.GetLevel();
        }
    }
}   