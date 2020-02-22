using System;
using System.Configuration;
using System.IO;

namespace RoomAid.ServiceLayer
{
    public class LogManager
    {
        private readonly LogMessage _message;
        public LogManager(LogMessage msg)
        {
            _message = msg;
        }

        // <Summary>
        // Write the created log to both data store locations.
        // If either fails, delete the log from the successful location to ensure that 
        // both logs have the same data.
        // </Summary>
        public bool WriteLog()
        {
            ILogHandler DSHandler = new DataStoreHandler(_message);
            ILogHandler FHandler = new FileHandler(_message);

            try
            {
                // Log to datastore
                if (!DSHandler.WriteLog()) // Returns false if logging has failed
                {
                    if (!Retry(DSHandler.WriteLog)) // Returns false if three tries fail
                    {
                        throw new IOException(); // Call Daniel's exception handler??
                    }
                }

                // Log to file
                if (!FHandler.WriteLog()) // Returns false if logging has failed
                {
                    if(!Retry(FHandler.WriteLog)) // Returns false if three tries fail
                    {
                        throw new IOException(); // Call Daniel's exception handler??
                    }
                }
                
            }
            catch (IOException)
            {
                // Delete log entry
                if (!DSHandler.DeleteLog()) // Returns false if deleting has failed
                {
                    if (!Retry(DSHandler.DeleteLog)) // Returns false if three tries fail
                    {
                        throw new IOException(); // Call Daniel's exception handler??
                    }
                }

                // Delete log file
                if (!FHandler.DeleteLog()) // Returns false if deleting has failed
                {
                    if (!Retry(FHandler.DeleteLog)) // Returns false if three tries fail
                    {
                        throw new IOException(); // Call Daniel's exception handler??
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// This will try to log the log three times, as required by the business rules
        /// </summary>
        /// <param name="method">The method that needed to be retried</param>
        /// <param name="msg">The log message that is being retried for logging</param>
        /// <returns>True if the retry is successful, false otherwise</returns>
        private static bool Retry(Func<bool> method)
        {
            //Set a bool as the retry result
            bool retrySuccess = false;

            //Retry until it reached the limit time of retry or it successed
            int retryLimit = Int32.Parse(ConfigurationManager.AppSettings["retryLimit"]);
            for (int i = 0; i < retryLimit; i++)
            {
                //Call method again to check if the certain method can be executed successfully
                retrySuccess = method();

                // If the result is true, then stop the retry
                if (retrySuccess)
                {
                    break;
                }
            }

            return retrySuccess; // Return false
        }
    }
}
