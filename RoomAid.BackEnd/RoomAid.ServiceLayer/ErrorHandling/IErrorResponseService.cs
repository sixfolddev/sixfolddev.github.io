using System;

namespace RoomAid.ServiceLayer
{
    public interface IErrorResponseService 
    {
        /// <summary>
        /// Helps with organizing differennt types of responses depending on threat level
        /// </summary>
        /// 
        AnalyzedError GetResponse();

    }
}
