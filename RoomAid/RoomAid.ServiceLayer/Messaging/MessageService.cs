/*
 * This class represents a messaging service that gives functionality for sending a message
 * to a message queue, and reading a message from a database.
 */
using System;
using System.Collections;

namespace RoomAid.ServiceLayer
{
    public class MessageService
    {
        private readonly IQueueHandler _queue;

        public MessageService()
        {
            _queue = new MSMQHandler(); // NOTE: No error is thrown if this line is excluded
        }

        public void SendMessage(IMessage message)
        {
            try
            {
                _queue.Send(message); // Send message to queue
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // TODO: implement
        public void ReadMessage()
        {
            //read from a database (non-specific)
        }
    }
}
