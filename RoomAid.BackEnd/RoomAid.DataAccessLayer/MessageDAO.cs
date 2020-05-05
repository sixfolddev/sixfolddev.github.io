using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace RoomAid.DataAccessLayer
{
    public class MessageDAO : ISqlDAO
    {
        private readonly string _connectionString;
        // This represents the max number of columns in a single row that can be retrieved from a select query
        private const int _MaxColumns = 20;
        public MessageDAO()
        {
            _connectionString = Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User); // Default connection string
        }

        public MessageDAO(string connection)
        {
            _connectionString = connection;
        }

        public int RunCommand(SqlCommand command)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction trans = connection.BeginTransaction();
                try
                {
                    using (command)
                    {
                        command.Connection = connection;
                        command.Transaction = trans;
                        int rowsAffected = command.ExecuteNonQuery();
                        trans.Commit();
                        return rowsAffected;
                    }
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
        }
        
        public int RunCommand(List<SqlCommand> commands)
        {
            int rowsAffected = 0;
            foreach (SqlCommand cmd in commands)
            {
                rowsAffected += RunCommand(cmd);
            }

            return rowsAffected;
        }

        public Object RetrieveOneColumn(SqlCommand command)
        {
            Object data;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction trans = connection.BeginTransaction();
                try
                {
                    using (command)
                    {
                        command.Connection = connection;
                        command.Transaction = trans;
                        data = command.ExecuteScalar();
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
            return data;
        }

        public IList<string> RetrieveOneRow(SqlCommand command)
        {
            IList<string> listOfColumns = new List<string>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    using (command)
                    {
                        command.Connection = connection;
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            if (dataReader.Read())
                            {
                                Object[] objectArray = new Object[_MaxColumns];
                                int count = dataReader.GetValues(objectArray); // Count will store the number of columns retrieved
                                for (int i = 0; i < count; i++)
                                {
                                    listOfColumns.Add(objectArray[i].ToString()); // Add all the columns associated with one row
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return listOfColumns;
        }

        public IList<IList<string>> RetrieveMultipleRows(SqlCommand command)
        {
            IList<IList<string>> listOfRows = new List<IList<string>>();
            int row = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    using (command)
                    {
                        command.Connection = connection;
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Object[] objectArray = new Object[_MaxColumns];
                                int count = dataReader.GetValues(objectArray); // Count will store the number of columns retrieved
                                IList<string> list = new List<string>();
                                for (int i = 0; i < count; i++)
                                {
                                    list.Add(objectArray[i].ToString()); // Add all the columns associated with one row
                                }
                                listOfRows.Add(list);
                                row++; // Iterate to next row
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return listOfRows;
        }

        public bool SendToDB(List<SqlCommand> commands)
        {
            try
            {
                int rowsAffected = RunCommand(commands);
                if (rowsAffected == commands.Count)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return false;
        }

        public bool SendToDB(SqlCommand command)
        {
            try
            {
                int rowsAffected = RunCommand(command);
                if (rowsAffected == 1)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return false;
        }

        /// <summary>
        /// Retrieves from a SQL database the contents of a message. The contents consist of: previous message ID, sender ID, sent date, and message body or invitation response.
        /// The receiver ID and message ID of a message are omitted from the query because those are already known. Read status is also omitted because it will always be true in this case.
        /// </summary>
        /// <param name="receiverID">ID of the user who is retrieving messages from their inbox</param>
        /// <param name="messageID">ID of a message</param>
        /// <param name="isGeneral">Boolean value indicating whether a message is a general message or not</param>
        /// <returns>Contents of a message</returns>
        public IList<string> ReadMessageFromDB(int receiverID, int messageID, bool isGeneral)
        {
            IList<string> messageContent;
            try
            {
                var command = new SqlCommand(ConfigurationManager.AppSettings["querySelectMessage"]); // Query to select previous message ID, sender ID, and sent date of messages
                command.Parameters.AddWithValue("@rcvid", receiverID);
                command.Parameters.AddWithValue("@msgid", messageID);
                messageContent = RetrieveOneRow(command);

                if (isGeneral) // If message is a general message
                {
                    command = new SqlCommand(ConfigurationManager.AppSettings["querySelectGeneralMessageBody"]); // Query to select message body
                }
                else // If message is an invitation
                {
                    command = new SqlCommand(ConfigurationManager.AppSettings["querySelectInvitationResponse"]); // Query to select invitation response
                }
                command.Parameters.AddWithValue("@rcvid", receiverID);
                command.Parameters.AddWithValue("@msgid", messageID);
                string content = RetrieveOneColumn(command).ToString();
                messageContent.Add(content); // Both commands above return only one column
            }
            catch (Exception e)
            {
                throw e;
            }

            return messageContent;
        }

        /// <summary>
        /// Retrieves from a SQL database message details that make up a message listing in an inbox. Each message listing contains these details in this order: 
        /// Message ID, read status of the message, sent date, first name, and last name.
        /// </summary>
        /// <param name="receiverID">ID of the user who is retrieving messages from their inbox</param>
        /// <param name="isGeneral">Boolean value indicating whether a message is a general message or not</param>
        /// <returns>A list (message listings) of a list of strings (message listing details)</returns>
        public IList<IList<string>> ReadInboxFromDB(int receiverID, bool isGeneral)
        {
            IList<IList<string>> listOfMessagesDetails; // List of messages' details in an inbox; each listing contains messageID, isRead, sentDate, sender first name, and sender last name
            /* <add key ="querySelectInbox" value="SELECT MessageID, IsRead, SentDate, SenderID FROM dbo.InboxMessages WHERE SysID = @rcvid AND IsGeneral = @general"/>
             * <add key ="querySelectUserName" value="SELECT FirstName, LastName FROM dbo.Users WHERE SysID = @sendid"/>
             */
            var command = new SqlCommand(ConfigurationManager.AppSettings["querySelectInbox"]); // Query to select message ID, sent date, and sender ID of messages
            const int SenderIDIndex = 3;
            command.Parameters.AddWithValue("@rcvid", receiverID);
            command.Parameters.AddWithValue("@general", isGeneral);
            try
            {
                listOfMessagesDetails = RetrieveMultipleRows(command);
                for(int i = 0; i < listOfMessagesDetails.Count; i++)
                {
                    int senderID = Int32.Parse(listOfMessagesDetails[i][SenderIDIndex]); // Previous query retrieves senderID as the fourth column/value
                    command = new SqlCommand(ConfigurationManager.AppSettings["querySelectUserName"]); // Query to select first and last name of one user by system ID (guaranteed one since SysID is unique)
                    command.Parameters.AddWithValue("@sendid", senderID);
                    IList<string> senderInfo = RetrieveOneRow(command);
                    listOfMessagesDetails[i].RemoveAt(SenderIDIndex); // Remove SenderID
                    listOfMessagesDetails[i].Add(senderInfo[0]); // Add first name to list of details of current message
                    listOfMessagesDetails[i].Add(senderInfo[1]); // Add last name to list of details of current message
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return listOfMessagesDetails;
        }
    }
}
