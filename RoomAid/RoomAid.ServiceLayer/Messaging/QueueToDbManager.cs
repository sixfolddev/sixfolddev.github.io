using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Messaging;
using System.Text;

namespace RoomAid.ServiceLayer.Messaging
{
    public class QueueToDbManager
    {
        public QueueToDbManager()
        {

        }

        public static void Send(string queueName, string message)
        {
            if (queueName == null)
            {
                throw new Exception("The message queue name cannot be null.");
            }
            CreateQueue(queueName); // Creates the queue on the system
            try
            {
                using (var messageQueue = new MessageQueue(queueName, QueueAccessMode.Send))
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
