using System;
using System.Configuration;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.ServiceLayer;

namespace RoomAid.Logging.Tests
{
    [TestClass]
    public class DsHandlerTest
    {
        private static readonly LogMessage _msg = new LogMessage(
            DateTime.UtcNow,
            ConfigurationManager.AppSettings["testClass"],
            ConfigurationManager.AppSettings["testMethod"],
            LogLevels.Levels.None,
            ConfigurationManager.AppSettings["testUser"],
            ConfigurationManager.AppSettings["testSession"],
            ConfigurationManager.AppSettings["testText"]);

        [TestMethod]
        public void WriteLog_NewFileCreationAndWrite_Pass()
        {
            //Arrange
            var dsHandler = new DataStoreHandler(_msg);
            var expected = true;
            var actual = false;
            //Act
            if(dsHandler.WriteLog())
            {
                actual = true;
            }
            //Assert
            Assert.IsTrue(expected == actual);
        }
        [TestMethod]
        public void DeleteLog_DeleteExistingLog_Pass()
        {
            //Arrange
            var dsHandler = new DataStoreHandler(_msg);
            var expected = true;
            var actual = false;
            //Act
            if (dsHandler.WriteLog())
            {
                if (dsHandler.DeleteLog())
                {
                    actual = true;
                }
            } 
            //Assert
            Assert.IsTrue(expected == actual);
        }
    }
}
