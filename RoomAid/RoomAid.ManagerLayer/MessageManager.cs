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

        // TODO: change type void and do something with getinbox() return
        public IList<IList<string>> GetAllMessages(int receiverID)
        {
            return _messageService.GetInbox(_dao, receiverID, true);
        }

        // TODO: change type void and do something with getinbox() return
        public IList<IList<string>> GetAllInvitations(int receiverID)
        {
            return _messageService.GetInbox(_dao, receiverID, false);
        }

        public bool SendMessage(int receiverID, int senderID, string messageBody)
        {
            if(messageBody == "" || messageBody == null)
            {
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
            if (messageBody == "" || messageBody == null)
            {
                return false; // How to accept a boolean from front-end?
            }
            var reply = new GeneralMessage(receiverID, prevMessageID, senderID, GetDateTime.GetUTCNow(), messageBody);
            // NOTE: Controller should call read as soon user selects message to read, so I shouldn't have to call Read() here
            Send(reply);
            return true;
        }

        public void ReplyInvitation(int receiverID, int prevMessageID, int senderID, bool accepted)
        {
            var reply = new Invitation(receiverID, prevMessageID, senderID, GetDateTime.GetUTCNow());
            if (accepted)
            {
                reply.IsAccepted = true;
            }
            Send(reply);
        }

        public IMessage ReadMessage(int receiverID, int messageID)
        {
            // Assign strings to corresponding message fields
            IList<string> messageContent = (List<string>)_messageService.ReadMessage(_dao, receiverID, messageID);
            //int receiverID = Int32.Parse(messageContent[0]); // Should have these already?
            //int messageID = Int32.Parse(messageContent[1]); // Should have these already?
            int prevMessageID = Int32.Parse(messageContent[2]); // If they click on this, call ReadMessage() again
            int senderID = Int32.Parse(messageContent[3]);
            bool read = true; // Always true because a message is being read
            DateTime date = DateTime.Parse(messageContent[5]);
            string content = messageContent[6];
            // Create message
            var message = new GeneralMessage(receiverID, prevMessageID, senderID, date, content)
            {
                // Set MessageID and IsRead to values above
                MessageID = messageID,
                IsRead = read
            };
            return message;
        }

        // TODO: implement
        public IMessage ReadInvitation(int receiverID, int messageID)
        {
            // Assign strings to corresponding message fields
            IList<string> invitationContent = (List<string>)_messageService.ReadMessage(_dao, receiverID, messageID);
            //int receiverID = Int32.Parse(invitationContent[0]);
            //int messageID = Int32.Parse(invitationContent[1]);
            int prevMessageID = Int32.Parse(invitationContent[2]);
            int senderID = Int32.Parse(invitationContent[3]);
            bool read = true; // Always true because a message is being read
            DateTime date = DateTime.Parse(invitationContent[5]);
            // NOTE: IsAccepted is false by default so 7th value in list is unnecessary
            // Create invitation
            var invitation = new Invitation(receiverID, prevMessageID, senderID, date)
            {
                // Set MessageID and IsRead to values above
                MessageID = messageID,
                IsRead = read
            };

            return invitation;
        }

        private void Send(IMessage message)
        {
            try
            {
                _messageService.SendMessage(message);
            }
            catch (Exception e)
            {
                _errorHandler.Handle(e);
            }
        }

    }
}