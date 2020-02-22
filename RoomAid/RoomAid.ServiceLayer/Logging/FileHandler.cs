using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace RoomAid.ServiceLayer
{
    public class FileHandler : ILogHandler
    {
        private readonly string _directory;
        private readonly ILogFormatter _formatter;
        private readonly LogMessage _message;
        private readonly string _firstparam;

        /// <summary>
        /// Default constructor. Initializes a directory to store the log files and the formatter to
        /// format the log message into a single line format.
        /// </summary>
        public FileHandler(LogMessage msg)
        {
            _directory = ConfigurationManager.AppSettings["logStorage"]; // Temporary directory
            _formatter = new SingleLineFormatter();
            _message = msg;
            _firstparam = msg.Time.ToString();
        }

        /// <summary>
        /// Constructor with specified formatter passed in. Initializes a directory to store the log files 
        /// and the formatter to format the log message.
        /// </summary>
        public FileHandler(ILogFormatter format)
        {
            _directory = ConfigurationManager.AppSettings["logStorage"]; // Temporary directory
            _formatter = format;
        }

        /// <summary>
        /// Constructor with specified directory and formatter passed in. Initializes a directory to store 
        /// the log files and the formatter to format the log message.
        /// </summary>
        public FileHandler(string directory, ILogFormatter format)
        {
            _directory = directory;
            _formatter = format;
        }

        // TODO: Write async
        /// <summary>
        /// Writes a log entry to a .csv file. If it fails, tries again up to 3x before throwing an exception.
        /// </summary>
        /// <param name="logMessage">Log entry to be written to file</param>
        /// <returns>true if write is successful, false otherwise</returns>
        public bool WriteLog()
        {
            try
            {
                var directory = new DirectoryInfo(_directory);
                if (!directory.Exists)
                {
                    directory.Create();
                }
                string fileName = MakeFileNameByDate(_message);
                string path = Path.Combine(_directory, fileName);
                if (!File.Exists(path)) // If file doesn't exist, create and write parameter names as first line
                {
                    using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8)) // UTF8 Encoding recommended for .NET Framework 4.7.2
                    {
                        writer.WriteLine(_message.GetParamNames());
                    }
                }
                using (StreamWriter writer = new StreamWriter(path, true, Encoding.UTF8))
                {
                    writer.WriteLine(_formatter.FormatLog(_message));
                }
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// Searches if a log entry exists in a .csv file and deletes it. If it fails, tries again up to 3x 
        /// before throwing an exception.
        /// </summary>
        /// <param name="logMessage">Log entry to search for and delete</param>
        /// <returns>true if delete is successful, false otherwise</returns>
        public bool DeleteLog()
        {
            try
            {
                string fileName = MakeFileNameByDate(_message);
                string path = Path.Combine(_directory, fileName);
                string[] logEntries = File.ReadAllLines(path);
                for (var j = 0; j < logEntries.Length; j++)
                {
                    string[] tokens = logEntries[j].Split(',');
                    if (!(tokens[0].Equals(_firstparam)))
                    {
                        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8)) // Overwrite existing file
                        {
                            writer.WriteLine(_message);
                        }
                    }
                }
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public string MakeFileNameByDate(LogMessage logMessage)
        {
            return logMessage.Time.ToString(ConfigurationManager.AppSettings["dateFormat"]) + ConfigurationManager.AppSettings["logExtension"];
        }

    }
}
