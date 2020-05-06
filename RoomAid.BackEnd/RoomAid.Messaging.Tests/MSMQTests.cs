/* TODO: Consolidate same logic tests that have 1-2 different inputs (gen. msgs vs invites) */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomAid.ServiceLayer;

namespace RoomAid.Messaging.Tests
{
    [TestClass]
    public class MSMQTests
    {
        private readonly MSMQHandler _msmqHandler = new MSMQHandler();
        private const int _numMessages = 3; // For use when testing sending/receiving multiple messages

        /// <summary>
        /// Sends one general message to a queue and retrieves that message. If the same message is received
        /// then the test passes. Effectively tests the MSMQHandler Send() and Receive() methods.
        /// </summary>
        [TestMethod]
        public void SendAndReceiveOneMessage_GeneralMessage_Pass()
        {
            //Arrange
            IMessage message = new GeneralMessage(1, 2, GetDateTime.GetUTCNow(), "Test message");
            var incoming = new GeneralMessage();

            //Act
            try
            {
                _msmqHandler.Send(message);
                incoming = (GeneralMessage)_msmqHandler.Receive();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
            //Cleanup
            _msmqHandler.Purge();

            //Assert
            Assert.AreEqual(incoming.ToString(), message.ToString()); // Doesn't pass if not converted to String (even though seemingly equal)
        }

        /// <summary>
        /// Sends one invitation to a queue and retrieves that invitation. If the same invitation is received
        /// then the test passes. Effectively tests the MSMQHandler Send() and Receive() methods.
        /// </summary>
        [TestMethod]
        public void SendAndReceiveOneMessage_Invitation_Pass()
        {
            //Arrange
            IMessage invitation = new Invitation(1, 2, GetDateTime.GetUTCNow());
            var incoming = new Invitation();

            //Act
            try
            {
                _msmqHandler.Send(invitation);
                incoming = (Invitation)_msmqHandler.Receive();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
            //Cleanup
            _msmqHandler.Purge();

            //Assert
            Assert.AreEqual(incoming.ToString(), invitation.ToString()); // Doesn't pass if not converted to String (even though seemingly equal)
        }

        /// <summary>
        /// Sends multiple general messages to a queue and retrieves those messages. If the same messages are received in
        /// the same order, then the test passes. Effectively tests the MSMQHandler Send(), Peek() [on a non-empty queue],
        /// and Receive() methods.
        /// </summary>
        [TestMethod]
        public void SendMultipleMessages_RetrieveMessagesInSameOrder_GeneralMessages_Pass()
        {
            //Arrange
            bool isSuccess = false;
            IList messages = new List<IMessage>();
            var j = 0; // Counter to check number of incoming messages against number of messages sent

            //Act
            try
            {
                for (int i = 0; i < _numMessages; i++) // Creating 3 general messages to send to queue
                {
                    messages.Add(new GeneralMessage(i + 1, i + 2, GetDateTime.GetUTCNow(), "Test message" + i));
                    _msmqHandler.Send((IMessage)messages[i]);
                }
                while (_msmqHandler.Peek(1)) // 1 second should be enough to retrieve each message
                {
                    var incoming = _msmqHandler.Receive();
                    if (incoming.ToString().Equals(messages[j].ToString())) // Check each incoming message against messages list
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false; // If any are not equal, the messages are not in the same order or did not send properly
                    }
                    j++; // Increment incoming messages counter
                }
                if (j != _numMessages) // If the number of incoming messages doesn't equal the number of messages sent to the queue
                {
                    isSuccess = false;
                    Trace.WriteLine("Incorrect number of incoming messages");
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
            //Cleanup
            _msmqHandler.Purge();

            //Assert
            Assert.IsTrue(isSuccess);
        }

        /// <summary>
        /// Sends multiple invitations to a queue and retrieves those invitations. If the same invitations are received in
        /// the same order, then the test passes. Effectively tests the MSMQHandler Send(), Peek() [on a non-empty queue], 
        /// and Receive() methods.
        /// </summary>
        [TestMethod]
        public void SendMultipleMessages_RetrieveMessagesInSameOrder_Invitations_Pass()
        {
            //Arrange
            bool isSuccess = false;
            IList invitations = new List<IMessage>();
            var j = 0; // Counter to check number of incoming messages against number of messages sent

            //Act
            try
            {
                for (int i = 0; i < _numMessages; i++) // Creating 3 invitations to send to queue
                {
                    invitations.Add(new Invitation(i + 1, i + 2, GetDateTime.GetUTCNow()));
                    _msmqHandler.Send((IMessage)invitations[i]);
                }
                while (_msmqHandler.Peek(1)) // 1 second should be enough to retrieve each message
                {
                    var incoming = _msmqHandler.Receive();
                    if (incoming.ToString().Equals(invitations[j].ToString())) // Check each incoming message against messages list
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false; // If any are not equal, the messages are not in the same order or did not send properly
                    }
                    j++; // Increment incoming invitations counter
                }
                if (j != _numMessages) // If the number of incoming messages doesn't equal the number of messages sent to the queue
                {
                    isSuccess = false;
                    Trace.WriteLine("Incorrect number of incoming messages");
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
            //Cleanup
            _msmqHandler.Purge();

            //Assert
            Assert.IsTrue(isSuccess);
        }

        /// <summary>
        /// Peeks at an empty queue. If Peek() returns false (no message found in queue from peek), then this test passes.
        /// </summary>
        [TestMethod]
        public void PeekEmptyQueue_NotPass()
        {
            //Arrange
            var expected = false;
            var actual = true;
            _msmqHandler.Purge();

            //Act
            try
            {
                if (!_msmqHandler.Peek(1)) // If peek returns false
                {
                    actual = false;
                }
                
            }
            catch (ArgumentException ae)
            {
                Trace.WriteLine(ae.ToString());
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }

        /// <summary>
        /// Sends a message to a queue and purges (deletes all messages in) that queue. If queue is empty after purge,
        /// then this test passes.
        /// </summary>
        [TestMethod]
        public void PurgeQueue_Pass()
        {
            //Arrange
            var expected = true;
            var actual = false;
            IMessage message = new GeneralMessage(1, 2, GetDateTime.GetUTCNow(), "Test message");

            //Act
            try
            {
                _msmqHandler.Send(message);
                if (_msmqHandler.Peek(1)) // If message is sent to queue (a message exists on queue)
                {
                    _msmqHandler.Purge(); // Then purge
                    if (!_msmqHandler.Peek(1)) // If queue is now empty
                    {
                        actual = true; // Actual = true;
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }

            //Assert
            Assert.IsTrue(expected == actual);
        }
    }
}
