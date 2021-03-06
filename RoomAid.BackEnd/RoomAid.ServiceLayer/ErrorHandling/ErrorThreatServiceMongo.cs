﻿using MongoDB.Driver;
using System;

namespace RoomAid.ServiceLayer
{
    /// <summary>
    /// Continuation of ErrorThreatService for MongoSpecificExceptions
    /// </summary>
    public partial class ErrorThreatService
    {
        public ErrorThreatService()
        { }

        /// <summary>
        /// GetThreatLevel of MongoConnectionException
        /// </summary>
        /// <param name="exceptione"></param>
        /// <returns>Level</returns>
        public LogLevels.Levels GetThreatLevel(MongoConnectionException exceptione)
        {
            return LogLevels.Levels.Error;
        }
 
        /// <summary>
        /// GetThreatLevel of MongoCursorNotFoundException
        /// </summary>
        /// <param name="exceptione"></param>
        /// <returns>Level</returns>
        public LogLevels.Levels GetThreatLevel(MongoCursorNotFoundException exceptione)
        {
            return LogLevels.Levels.Error;
        }

       
        

        /// <summary>
        /// GetThreatLevel of Generic MongoException
        /// </summary>
        /// <param name="exceptione"></param>
        /// <returns>Level</returns>
        public LogLevels.Levels GetThreatLevel(MongoException exceptione)
        {
            return LogLevels.Levels.Warning;
        }

        
    }
}