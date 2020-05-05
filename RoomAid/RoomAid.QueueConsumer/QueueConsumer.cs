using RoomAid.DataAccessLayer;
using RoomAid.ManagerLayer;
using RoomAid.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;

namespace RoomAid.QueueConsumer
{
    public class QueueConsumer
    {
        private static IQueueHandler _queue = new MSMQHandler();
        private static ErrorController _errorHandler;

        static void Main(string[] args)
        {
            double timeout = 1;
            int sleepTime = 10000;

            while(true)
            {
                if(!_queue.Peek(timeout))
                {
                    Thread.Sleep(sleepTime); // Sleep for 10 seconds
                    continue;
                }
                IMessage message = (IMessage)_queue.Receive();
                try
                {
                    SendToDB(message);
                }
                catch (Exception e)
                {
                    _errorHandler.Handle(e);
                }
            }
        }

        public static void SendToDB(IMessage message)
        {
            var dao = new MessageDAO();
            bool success;
            try
            {
                // TODO: Create commands
                /*
                 * <add key ="queryCreateMessage" value ="INSERT INTO dbo.InboxMessages (SysID, MessageID, PrevMessageID, SenderID, IsRead, SentDate, IsGeneral) 
                 * VALUES (@rcvid, @msgid, @prevmsgid, @sendid, @read, @date, @general)"/>
                 * 
                 * <add key ="queryCreateGeneralMessage" value ="INSERT INTO dbo.GeneralMessages (SysID, MessageID, MessageBody) VALUES (@rcvid, @msgid, @msgbody)"/>
                 * 
                 * <add key ="queryCreateInvitation" value ="INSERT INTO dbo.Invitations (SysID, MessageID, IsAccepted) VALUES (@rcvid, @msgid, @accepted)"/>
                 */
                // Create InboxMessage
                var cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateMessage"]);
                cmd.Parameters.AddWithValue("@rcvid", message.ReceiverID);
                cmd.Parameters.AddWithValue("@prevmsgid", message.PrevMessageID);
                cmd.Parameters.AddWithValue("@sendid", message.SenderID);
                cmd.Parameters.AddWithValue("@read", message.IsRead);
                cmd.Parameters.AddWithValue("@date", message.SentDate);
                cmd.Parameters.AddWithValue("@general", message.IsGeneral);
                success = dao.SendToDB(cmd);

                // Get MessageID
                cmd = new SqlCommand(ConfigurationManager.AppSettings["queryGetMessageID"]);
                cmd.Parameters.AddWithValue("@rcvid", message.ReceiverID);
                cmd.Parameters.AddWithValue("@sendid", message.SenderID);
                cmd.Parameters.AddWithValue("@date", message.SentDate);
                cmd.Parameters.AddWithValue("@general", message.IsGeneral);
                int messageID = (int)dao.RetrieveOneColumn(cmd);

                // Create GeneralMessage 
                if (message.GetType().Equals(typeof(GeneralMessage)))
                {
                    GeneralMessage general = (GeneralMessage)message;
                    cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateGeneralMessage"]);
                    cmd.Parameters.AddWithValue("@rcvid", message.ReceiverID);
                    cmd.Parameters.AddWithValue("@msgid", messageID);
                    cmd.Parameters.AddWithValue("@msgbody", general.MessageBody);
                } // OR
                  // Create Invitation
                else if (message.GetType().Equals(typeof(Invitation)))
                {
                    Invitation invitation = (Invitation)message;
                    cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateInvitation"]);
                    cmd.Parameters.AddWithValue("@rcvid", message.ReceiverID);
                    cmd.Parameters.AddWithValue("@msgid", messageID);
                    cmd.Parameters.AddWithValue("@accepted", invitation.IsAccepted);
                }
                success = dao.SendToDB(cmd);

                if (!success)
                {
                    throw new Exception ("Incorrect number of rows affected");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
