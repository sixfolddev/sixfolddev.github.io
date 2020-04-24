using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Messaging;
using System.Text;

namespace RoomAid.ServiceLayer.Messaging
{
    public class DbToQueueManager
    {
        public DbToQueueManager()
        {

        }

        [SqlProcedure]
        public static void Send(SqlString queueName, SqlString message)
        {
            var queue = queueName.Value;
            if (queueName == null || queue == null)
            {
                throw new Exception("The message queue name cannot be null.");
            }
            CreateQueue(queue); // Creates the queue on the system
            try
            {
                using (var messageQueue = new MessageQueue(queue, QueueAccessMode.Send))
                {
                    
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void CreateQueue(string queuePath)
        {
            try
            {
                if (!MessageQueue.Exists(queuePath))
                {
                    MessageQueue.Create(queuePath);
                }
                else
                {
                    var stringBuilder = new StringBuilder(queuePath);
                    stringBuilder.Append(" already exists. Using existing queue.");
                    Trace.Write(stringBuilder.ToString());
                }
            }
            catch (MessageQueueException e)
            {
                throw e;
            }
        }
        
    }
}
