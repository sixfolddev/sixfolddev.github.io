using RoomAid.DataAccessLayer;
using RoomAid.ServiceLayer;
using System;

namespace RoomAid.ManagerLayer
{
    public class MessageManager
    {
        private readonly ISqlDAO _dao;
        private readonly MessageService _messageService;
        private readonly ErrorController _errorHandler;

        public MessageManager()
        {
            _messageService = new MessageService();
            _errorHandler = new ErrorController();
            // why are there no errors with the methods even when I don't instantiate them?
        }

        public bool SendMessage(int receiverID, int senderID, string messageBody)
        {
            if(messageBody == "" || messageBody == null)
            {
                return false; // How to accept a boolean from front-end?
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
            //Call ReadMessage? set isread = true again to be safe?
            Send(reply);
            return true;
        }

        public void ReplyInvitation(int receiverID, int prevMessageID, int senderID, bool accepted)
        {
            var reply = new Invitation(receiverID, prevMessageID, senderID, GetDateTime.GetUTCNow());
            //Call ReadInvitation? set isread = true again to be safe?
            if (accepted)
            {
                reply.IsAccepted = true;
            }
            Send(reply);
        }

        // TODO: implement
        public void ReadMessage()
        {
            // Set IsRead = true;
        }

        // TODO: implement
        public void ReadInvitation()
        {
            // Set IsRead = true;
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
