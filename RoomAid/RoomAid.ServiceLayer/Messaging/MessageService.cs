using System;
using System.Collections;

namespace RoomAid.ServiceLayer.Messaging
{
    class MessageService
    {
        private MSMQHandler _queue;

        public MessageService()
        {
        }

        public bool SendMessage(int receiverID, int senderID, DateTime date, string message)
        {
            //create message
            var newMessage = new GeneralMessage(receiverID, senderID, date, message);

            //send to msmq
            

            //send to db

            return false; // TODO: implement
        }

        public bool ReadMessage()
        {
            return false; // TODO: implement
        }

        public bool ReplyMessage()
        {
            return false; // TODO: implement
        }

        public bool SendInvitation()
        {
            return false; // TODO: implement
        }

        public bool ReadInvitation()
        {
            return false; // TODO: implement
        }

        public bool ReplyInvitation()
        {
            return false; // TODO: implement
        }
    }
}
