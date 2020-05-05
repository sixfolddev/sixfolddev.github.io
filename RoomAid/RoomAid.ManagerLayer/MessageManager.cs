using RoomAid.DataAccessLayer;
using RoomAid.ServiceLayer;
using System;
using System.Collections.Generic;

namespace RoomAid.ManagerLayer
{
    public class MessageManager
    {
        private readonly MessageDAO _dao;
        private readonly MessageService _messageService;
        private readonly ErrorController _errorHandler;

        public MessageManager()
        {
            // NOTE: No errors with the methods even when I don't instantiate them. I think errors will happen when I run this without them.
            _dao = new MessageDAO();
            _messageService = new MessageService();
            _errorHandler = new ErrorController();
        }

        // list of messages' details; each message contains the details: messageid, isread, sentdate, first name, last name [IN THIS ORDER]
        public IList<MessageListing> GetAllMessages(int receiverID)
        {
            try
            {
                IList<IList<string>> listOfMessagesDetails = _messageService.GetInbox(_dao, receiverID, true);
                var messageInbox = new List<MessageListing>(); // Container for all message listings in the inbox
                foreach (IList<string> message in listOfMessagesDetails)
                {
                    var MessageListing = new MessageListing()
                    {
                        MessageID = Int32.Parse(message[0]),
                        IsRead = bool.Parse(message[1]),
                        SentDate = DateTime.Parse(message[2]),
                        FirstName = message[3],
                        LastName = message[4]
                    };
                    messageInbox.Add(MessageListing);
                }
                return messageInbox;
            }
            catch (Exception e)
            {
                _errorHandler.Handle(e);
                return null; // NOTE: should this be a different return type? If kept, controller should check if null
            }
        }

        // TODO: change type void and do something with getinbox() return
        public IList<MessageListing> GetAllInvitations(int receiverID)
        {
            try
            {
                IList<IList<string>> listOfMessagesDetails = _messageService.GetInbox(_dao, receiverID, false);
                var invitationInbox = new List<MessageListing>(); // Container for all message listings in the inbox
                foreach (IList<string> message in listOfMessagesDetails)
                {
                    var MessageListing = new MessageListing()
                    {
                        MessageID = Int32.Parse(message[0]),
                        IsRead = bool.Parse(message[1]),
                        SentDate = DateTime.Parse(message[2]),
                        FirstName = message[3],
                        LastName = message[4]
                    };
                    invitationInbox.Add(MessageListing);
                }
                return invitationInbox;
            }
            catch (Exception e)
            {
                _errorHandler.Handle(e);
                return null; // NOTE: should this be a different return type? If kept, controller should check if null
            }
        }

        public bool SendMessage(int receiverID, int senderID, string messageBody)
        {
            if(messageBody == "" || messageBody == null) // NOTE: do check on front=end?
            {
                // TODO: throw new messagebody null exception?
                return false; // TODO: How to accept a boolean from front-end?
            }
            var newMessage = new GeneralMessage(receiverID, senderID, GetDateTime.GetUTCNow(), messageBody);
            Send(newMessage);
            return true;
        }

        public void SendInvitation(int receiverID, int senderID)
        {
            var invitation = new Invitation(receiverID, senderID, GetDateTime.GetUTCNow());
            Send(invitation);
        }

        public bool ReplyMessage(int receiverID, int prevMessageID, int senderID, string messageBody)
        {
            if (messageBody == "" || messageBody == null) // NOTE: do check on front=end?
            {
                // TODO: throw new messagebody null exception?
                return false; // How to accept a boolean from front-end?
            }
            var reply = new GeneralMessage(receiverID, prevMessageID, senderID, GetDateTime.GetUTCNow(), messageBody);
            return Send(reply);
        }

        public bool ReplyInvitation(int receiverID, int prevMessageID, int senderID, bool accepted)
        {
            var reply = new Invitation(receiverID, prevMessageID, senderID, GetDateTime.GetUTCNow());
            if (accepted)
            {
                reply.IsAccepted = true;
            }
            return Send(reply);
        }

        public IMessage ReadMessage(int receiverID, int messageID)
        {
            // Assign strings to corresponding message fields
            IList<string> messageContent = (List<string>)_messageService.ReadMessage(_dao, receiverID, messageID, true);
            int prevMessageID = Int32.Parse(messageContent[0]); // TODO: If they click on this, call ReadMessage() again
            int senderID = Int32.Parse(messageContent[1]);
            bool read = true; // Always true because a message is being read
            DateTime date = DateTime.Parse(messageContent[2]);
            string content = messageContent[3];
            // Create message
            var message = new GeneralMessage(receiverID, prevMessageID, senderID, date, content)
            {
                // Set MessageID and IsRead to values above
                MessageID = messageID,
                IsRead = read
            };
            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiverID"></param>
        /// <param name="messageID"></param>
        /// <returns></returns>
        public IMessage ReadInvitation(int receiverID, int messageID)
        {
            // Assign strings to corresponding message fields
            IList<string> invitationContent = (List<string>)_messageService.ReadMessage(_dao, receiverID, messageID, false);
            int prevMessageID = Int32.Parse(invitationContent[0]);
            int senderID = Int32.Parse(invitationContent[1]);
            bool read = true; // Always true because a message is being read
            DateTime date = DateTime.Parse(invitationContent[2]);
            bool accepted = bool.Parse(invitationContent[3]);
            // Create invitation
            var invitation = new Invitation(receiverID, prevMessageID, senderID, date)
            {
                // Set MessageID and IsRead to values above
                MessageID = messageID,
                IsRead = read,
                IsAccepted = accepted
            };
            return invitation;
        }

        /// <summary>
        /// Sends a message using the message service
        /// </summary>
        /// <param name="message">Message to be sent</param>
        /// <returns>true if message is successfully sent; false otherwise</returns>
        private bool Send(IMessage message)
        {
            try
            {
                _messageService.SendMessage(message);
                return true;
            }
            catch (Exception e)
            {
                _errorHandler.Handle(e);
                return false;
            }
        }

    }
}