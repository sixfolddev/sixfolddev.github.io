using System;
using RoomAid.ServiceLayer;

namespace RoomAid.ManagerLayer
{
    //Levels are used to determine severity of things logged, as well as helping determine how to handle errors 

    /// <summary>
    /// Warning Controller is the Large Controller that handles All operations that are required by 
    /// Warning Handling, divvying out tasks as needed to separate managers
    /// It is passed in any exception e, where individual services down the line will 
    /// gather more accurate info and pass back 
    /// </summary>
    public class ErrorController
    {
        public AnalyzedError Err
        { get;set; }

        public ErrorController() {}

        /// <summary>
        /// Handles the Exception contained within the object by assigning managers to the different steps of the tasks
        /// </summary>
        public void Handle(Exception exception)
        {
            
            var ThreatManager = new ErrorThreatManager(exception);
            var Error = ThreatManager.GetThreatLevel();
            ErrorResponseManager ResponseManager = new ErrorResponseManager();
            Err = ResponseManager.GetResponse(Error);
        }
        
        


    }
}