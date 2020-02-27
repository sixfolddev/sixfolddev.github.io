using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.ServiceLayer;

namespace RoomAid.Logging.Tests
{
    [TestClass]
    public class FileHandlerTest
    {
        // Log Storage directory
        private readonly string _logStorage = ConfigurationManager.AppSettings["logStorage"];
        private readonly ILogFormatter _formatter = new SingleLineFormatter();
        private static readonly LogMessage _msg = new LogMessage(
            DateTime.UtcNow,
            ConfigurationManager.AppSettings["testClass"],
            ConfigurationManager.AppSettings["testMethod"],
            LogLevels.Levels.None,
            ConfigurationManager.AppSettings["testUser"],
            ConfigurationManager.AppSettings["testSession"],
            ConfigurationManager.AppSettings["testText"]);
        private static readonly FileHandler fileHandler = new FileHandler(_msg);
        private readonly string _fileName = fileHandler.MakeFileNameByDate(_msg);

        [TestMethod]
        public void WriteLog_NewFileIsCreatedAndWrites_Pass()
        {
            //Arrange
            string path = Path.Combine(_logStorage, _fileName);
            var expected = true;
            var actual = false;

            //Act
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            if (fileHandler.WriteLog())
            {
                if (File.Exists(path)) // New file created
                {
                    string[] entries = File.ReadAllLines(path);
                    string message = _formatter.FormatLog(_msg);
                    var lastEntry = entries.Length - 1;
                    if (entries[lastEntry].Equals(message))
                    {
                        actual = true;
                    }
                }
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void WriteLog_LogEntryAppends_Pass()
        {
            //Arrange
            string path = Path.Combine(_logStorage, _fileName);
            var expected = true;
            var actual = false;

            //Act
            if (fileHandler.WriteLog())
            {
                string[] entries = File.ReadAllLines(path);
                string message = _formatter.FormatLog(_msg);
                var lastEntry = entries.Length - 1;
                if (entries[lastEntry].Equals(message))
                {
                    actual = true;
                }
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void DeleteLog_LogEntryFoundAndDeleted_Pass()
        {
            //Arrange
            string path = Path.Combine(_logStorage, _fileName);
            var expected = true;
            var actual = false;

            //Act
            if (fileHandler.WriteLog())
            {
                string[] entries = File.ReadAllLines(path);
                string message = _formatter.FormatLog(_msg);
                var lastEntry = entries.Length - 1;
                if (entries[lastEntry].Equals(message))
                {
                    fileHandler.DeleteLog();
                }
                entries = File.ReadAllLines(path);
                lastEntry = entries.Length - 1;
                if (!entries[lastEntry].Equals(message))
                {
                    actual = true;
                }
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void DeleteLog_LogFileUnaltered_Pass()
        {
            //Arrange
            string path = Path.Combine(_logStorage, _fileName);
            var expected = true;
            var actual = false;

            //Act
            string[] entries = File.ReadAllLines(path);
            string message = _formatter.FormatLog(_msg);
            if(!entries.Any(message.Equals))
            {
                fileHandler.DeleteLog();
                entries = File.ReadAllLines(path);
                var lastEntry = entries.Length - 1;
                if (!entries[lastEntry].Equals(message))
                {
                    actual = true;
                }
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }
    }   
}