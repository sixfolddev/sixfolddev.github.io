using System;
using System.Messaging;
using System.Text;

namespace RoomAid.ServiceLayer.Messaging
{
    public class MSMQHandler : IQueueHandler
    {
        private string _queuePath;

        public MSMQHandler()
        {
            _queuePath = @".\PRIVATE$\MessageQueue"; // Default path
            Create(_queuePath);
        }

        public MSMQHandler(string qpath)
        {
            _queuePath = qpath;
            Create(_queuePath);
        }

        public void Send(string message)
        {
            try
            {
                using (var messageQueue = new MessageQueue(_queuePath, QueueAccessMode.Send)) // Should I be granting exclusive send access..? Benefits? Drawbacks?
                {
                    if(messageQueue.Transactional)
                    {
                        using (var transaction = new MessageQueueTransaction())
                        {
                            transaction.Begin();
                            // Is it necessary to set a label for my messagequeue? Read from a book that trying
                            // to bind and send messages to a queue using the same label "guarantees" problems
                            messageQueue.Send(message, transaction); // vs. Send(obj, transtype)?
                            transaction.Commit();
                        }
                    }
                    else
                    {
                        messageQueue.Send(message);
                    }
                }
            }
            catch (Exception e) // Should catch MessageQueue exceptions/errors as well as Transaction exceptions/errors
            {
                throw e;
            }
        }

        public string Receive()
        {
            try
            {
                using (var messageQueue = new MessageQueue(_queuePath, QueueAccessMode.Receive)) // Do I need to make this new queue at a different path than the one for sending?
                {
                    Message incomingMessage;
                    messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                    if (messageQueue.Transactional)
                    {
                        using (var transaction = new MessageQueueTransaction())
                        {
                            transaction.Begin(); // Begin transaction
                            incomingMessage = messageQueue.Receive(TimeSpan.FromSeconds(1), transaction); // Recommended time span..?
                            transaction.Commit(); // Commit transaction
                        }
                    }
                    else
                    {
                        incomingMessage = messageQueue.Receive();
                    }
                    return incomingMessage.Body.ToString();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Creates a transactional message queue at the specified path
        /// </summary>
        /// <param name="path">Path of the queue to create</param>
        public void Create(string path)
        {
            if (_queuePath == null)
            {
                throw new ArgumentNullException();
            }
            try
            {
                if (!MessageQueue.Exists(path))
                {
                    MessageQueue.Create(path, true);
                }
                else
                {
                    var stringBuilder = new StringBuilder("A queue already exists at ");
                    stringBuilder.Append(path);
                    stringBuilder.Append(". Using existing queue.");
                    LogService.Log(stringBuilder.ToString());
                }
            }
            catch (MessageQueueException e)
            {
                throw e;
            }
        }

        public void Delete(string path)
        {
            if (_queuePath == null)
            {
                throw new ArgumentException();
            }
            try
            {
                MessageQueue.Delete(path);
            }
            catch (MessageQueueException e)
            {
                throw e;
            }
        }
    }
}
