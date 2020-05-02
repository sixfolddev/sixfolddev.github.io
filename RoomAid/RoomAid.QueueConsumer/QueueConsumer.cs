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
    class QueueConsumer
    {
        private static IQueueHandler queue = new MSMQHandler();
        private static ErrorController _errorHandler;

        static void Main(string[] args)
        {
            double timeout = 1;
            int sleepTime = 10000;

            while(true)
            {
                if(!queue.Peek(timeout))
                {
                    Thread.Sleep(sleepTime); // Sleep for 10 seconds
                    continue;
                }
                IMessage message = (IMessage)queue.Receive();
                SendToDB(message);
            }
        }

        private static void SendToDB(IMessage message)
        {
            var dao = new MessageDAO();
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
            List<SqlCommand> commands = new List<SqlCommand>();
            var cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateMessage"]);
            cmd.Parameters.AddWithValue("@rcvid", message.ReceiverID);
            cmd.Parameters.AddWithValue("@msgid", message.MessageID);
            cmd.Parameters.AddWithValue("@prevmsgid", message.PrevMessageID);
            cmd.Parameters.AddWithValue("@sendid", message.SenderID);
            cmd.Parameters.AddWithValue("@read", message.IsRead);
            cmd.Parameters.AddWithValue("@date", message.SentDate);
            cmd.Parameters.AddWithValue("@general", message.IsGeneral);
            commands.Add(cmd);

            // Create GeneralMessage 
            if (message.GetType().Equals(typeof(GeneralMessage)))
            {
                GeneralMessage general = (GeneralMessage)message;
                cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateGeneralMessage"]);
                cmd.Parameters.AddWithValue("@rcvid", message.ReceiverID);
                cmd.Parameters.AddWithValue("@msgid", message.MessageID);
                cmd.Parameters.AddWithValue("@msgbody", general.MessageBody);
                commands.Add(cmd);
            } // OR
            // Create Invitation
            else if (message.GetType().Equals(typeof(Invitation)))
            {
                Invitation invitation = (Invitation)message;
                cmd = new SqlCommand(ConfigurationManager.AppSettings["queryCreateInvitation"]);
                cmd.Parameters.AddWithValue("@rcvid", message.ReceiverID);
                cmd.Parameters.AddWithValue("@msgid", message.MessageID);
                cmd.Parameters.AddWithValue("@accepted", invitation.IsAccepted);
                commands.Add(cmd);
            }

            try
            {
                bool success = dao.SendToDB(commands);
                if (!success)
                {
                    throw new Exception ("Incorrect number of rows affected");
                }
            }
            catch (Exception e)
            {
                _errorHandler.Handle(e);
            }
        }
    }
}
