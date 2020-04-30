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

        public bool Send(string message)
        {
            try
            {
                using (var messageQueue = new MessageQueue(_queuePath, QueueAccessMode.Send)) // Should I be granting exclusive send access..? Benefits? Drawbacks?
                {
                    // Is it necessary to set a label for my messagequeue? Read from a book that trying
                    // to bind and send messages to a queue using the same label "guarantees" problems

                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Receive(string message)
        {
            try
            {
                using (var messageQueue = new MessageQueue(_queuePath, QueueAccessMode.Receive))
                {

                }
                return true;
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
                throw new Exception("The message queue name cannot be null.");
            }
            try
            {
                MessageQueue.Create(path, true);

                // "If you try to create an already existing message queue, a MessageQueueException exception is thrown"
                /*
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
                */
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
