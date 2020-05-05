/*
 * This class represents a messaging service that gives functionality for sending a message to a message queue, 
 * reading the contents of a message from a database, and retrieving the details of message listings in an inbox from a database
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

        /// <summary>
        /// Default constructor. Initializes an instance of a queue.
        /// </summary>
        public MessageService()
        {
            _queue = new MSMQHandler(); // NOTE: No error is thrown if this line is excluded.
        }

        /// <summary>
        /// Sends a message to a message queue
        /// </summary>
        /// <param name="message">Message to be sent to the queue</param>
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

        /// <summary>
        /// Retrieves the contents of a message. The contents consist of: previous message ID, sender ID, read status, sent date, and message body or invitation response (in this order)
        /// The receiver ID and message ID of a message are omitted from the query because those are already known. Read status is also omitted because it will always be true in this case.
        /// </summary>
        /// <param name="receiverID">ID of the user who is retrieving messages from their inbox</param>
        /// <param name="messageID">ID of a message</param>
        /// <param name="isGeneral">Boolean value indicating whether a message is a general message or not</param>
        /// <returns>Contents of a message</returns>
        public IList<string> ReadMessage(MessageDAO dao, int receiverID, int messageID, bool isGeneral)
        {
            try
            {
                List<string> messageContent = (List<string>)dao.ReadMessageFromDB(receiverID, messageID, isGeneral);
                return messageContent;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // NOTE: Flaw in design resulting in need for specification of user-defined message type in service
        /// <summary>
        /// Retrieves a list of message listings' details that make up an inbox. Each message listing contains these details in this order: 
        /// Message ID, read status of the message, sent date, first name, and last name  
        /// </summary>
        /// <param name="dao">Object used to access messages from a database</param>
        /// <param name="receiverID">ID of the user who is retrieving messages from their inbox</param>
        /// <param name="isGeneral">Boolean value indicating whether a message is a general message or not</param>
        /// <returns>A list of message listing details</returns>
        public IList<IList<string>> GetInbox(MessageDAO dao, int receiverID, bool isGeneral)
        {
            try
            {
                return dao.ReadInboxFromDB(receiverID, isGeneral);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
