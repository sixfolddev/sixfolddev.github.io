using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RoomAid.DataAccessLayer
{
    public class MessageDAO : ISqlDAO
    {
        private readonly string _connectionString;
        public MessageDAO()
        {
            _connectionString = Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User); // Default connection string
        }

        public MessageDAO(string connection)
        {
            _connectionString = connection;
        }
        
        public int RunCommand(List<SqlCommand> commands)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction trans = connection.BeginTransaction();
                try
                {
                    foreach (SqlCommand cmd in commands)
                    {
                        cmd.Connection = connection;
                        cmd.Transaction = trans;
                        rowsAffected += cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
            return rowsAffected;
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

        // TODO: implement
        public IList<string> ReadOneFromDB(int receiverID, int messageID)
        {
            return new List<string>();
        }

        // TODO: implement
        public IList<IList<string>> ReadMultipleFromDB(int receiverID, bool isGeneral)
        {
            IList<IList<string>> listOfLists = new List<IList<string>>();
            // select messageid, sentdate, senderid from dbo.inboxmessages where receiverID = SysID and isGeneral = isGeneral,
            // end up with a outerlist of innerlists; each list carries messageid[0], sentdate[1], and senderid[2] in that order
            // for loop (outerlist.count) { sender = innerlist[2]; select first, last from dbo.Users where senderID = SysID;
            //       first = datareader.GetValue(0); last = datereader.GetValue(1); //Datareader should only have 1 result
            //       innerlist.RemoveAt(2); innerList.Add(first); innerList.Add(last); }

            // Return messageid, datetime of messages, first name, last name [IN THIS ORDER]
            return listOfLists;
        }
    }
}
