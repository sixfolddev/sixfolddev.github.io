/*
 * This class represents a messaging service that gives functionality for sending a message
 * to a message queue, and reading the contents of a message from a database.
 */
using RoomAid.DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RoomAid.ServiceLayer
{
    public class MessageService
    {
        private readonly IQueueHandler _queue;

        public MessageService()
        {
            _queue = new MSMQHandler(); // NOTE: No error is thrown if this line is excluded.
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

        public IList<string> ReadMessage(MessageDAO dao, int receiverID, int messageID)
        {
            List<string> messageContent = (List<string>)dao.ReadOneFromDB(receiverID, messageID);
            return messageContent;
        }

        // TODO: change type void and do something with getinbox() return
        // NOTE: Flaw in design resulting in need for specification of user-defined message type in service
        public IList<IList<string>> GetInbox(MessageDAO dao, int receiverID, bool isGeneral)
        {
            return dao.ReadMultipleFromDB(receiverID, isGeneral);
        }
    }
}
