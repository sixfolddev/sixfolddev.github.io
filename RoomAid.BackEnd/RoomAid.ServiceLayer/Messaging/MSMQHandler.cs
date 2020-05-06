/*
 * This class handles the basic functionality of peeking, sending, receiving, and purging an MSMQ. A queue must be created 
 * on the system prior to using these methods; otherwise errors are thrown. Implements IQueueHandler.
 */
using System;
using System.Messaging;

namespace RoomAid.ServiceLayer
{
    public class MSMQHandler : IQueueHandler
    {
        private readonly string _queuePath; // Path of the queue on the system
        private readonly Type[] typeArray; // Target types for message queue formatter

        /// <summary>
        /// Default constructor that sets the path for a message queue and the type array
        /// that will be used in the message queue formatter
        /// </summary>
        public MSMQHandler()
        {
            _queuePath = @".\PRIVATE$\MessageQueue"; // Default path
            typeArray = new Type[] { typeof(GeneralMessage), typeof(Invitation) };
        }

        /// <summary>
        /// Peeks at the first message in a queue and returns true if one exists
        /// </summary>
        /// <param name="timeoutInSeconds">Number of seconds to timeout the peek function if peek doesn't find a message</param>
        /// <returns>True if peek returns a copy of a message in the queue, false otherwise</returns>
        public bool Peek(double timeoutInSeconds)
        {
            try
            {
                using (var messageQueue = new MessageQueue(_queuePath, QueueAccessMode.Peek))
                {
                    if (messageQueue.Peek(TimeSpan.FromSeconds(timeoutInSeconds)) != null)
                    {
                        return true;
                    }
                }
            }
            catch (MessageQueueException) // HACK: Needed for handling queue timeout, but handles all MessageQueueExceptions by returning false
            {
                return false;
            }
            catch (ArgumentException ae) // NOTE: Is it better to use an exception filter (argument and messagequeue exceptions)?
            {
                throw ae;
            }
            return false;
        }

        /// <summary>
        /// Sends an IMessage object to a transactional message queue
        /// </summary>
        /// <param name="message">Message to send to the queue</param>
        public void Send(IMessage message)
        {
            var outgoingMessage = new Message();
            try
            {
                using (var messageQueue = new MessageQueue(_queuePath, QueueAccessMode.Send))
                {
                    messageQueue.Formatter = new XmlMessageFormatter(typeArray); // Sets the formatter for the outoing message to the queue
                    outgoingMessage.Body = message;

                    if (messageQueue.Transactional)
                    {
                        using (var transaction = new MessageQueueTransaction())
                        {
                            try
                            {
                                transaction.Begin();
                                messageQueue.Send(outgoingMessage, transaction);
                                transaction.Commit();
                            }
                            catch (MessageQueueException mqe)
                            {
                                transaction.Abort();
                                throw mqe;
                            }
                        }
                    }
                    else
                    {
                        messageQueue.Send(outgoingMessage);
                    }
                }
            }
            catch (MessageQueueException mqe)
            {
                throw mqe;
            }
            finally
            {
                outgoingMessage.Dispose();
            }
        }

        /// <summary>
        /// Receives and returns an IMessage object from a transactional message queue
        /// </summary>
        /// <returns>An IMessage object</returns>
        public IMessage Receive()
        {
            var incomingMessage = new Message();
            try
            {
                using (var messageQueue = new MessageQueue(_queuePath, QueueAccessMode.Receive))
                {
                    messageQueue.Formatter = new XmlMessageFormatter(typeArray); // Sets the formatter for the incoming message from the queue

                    if (messageQueue.Transactional)
                    {
                        using (var transaction = new MessageQueueTransaction())
                        {
                            try
                            {
                                transaction.Begin();
                                incomingMessage = messageQueue.Receive(TimeSpan.FromSeconds(1), transaction);
                                transaction.Commit();
                            }
                            catch (MessageQueueException mqe)
                            {
                                transaction.Abort();
                                throw mqe;
                            }  
                        }
                    }
                    else
                    {
                        while (messageQueue.Peek(TimeSpan.FromSeconds(1)) != null)
                        {
                            incomingMessage = messageQueue.Receive(TimeSpan.FromSeconds(1));

                        }
                    }
                    IMessage message = (IMessage)incomingMessage.Body;
                    return message;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                incomingMessage.Dispose();
            }
        }

        /// <summary>
        /// Deletes all messages in a queue
        /// </summary>
        public void Purge()
        {
            try
            {
                using (var messageQueue = new MessageQueue(_queuePath))
                {
                    messageQueue.Purge();
                }
            }
            catch (MessageQueueException mqe)
            {
                throw mqe;
            }
        }
    }
}
