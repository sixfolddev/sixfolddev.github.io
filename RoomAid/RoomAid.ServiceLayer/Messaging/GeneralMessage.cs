using System;

namespace RoomAid.ServiceLayer.Messaging
{
    public class GeneralMessage : IMessage
    {
        public int ReceiverID { get; set; }
        public int MessageID { get; set; }
        public int PrevMessageID { get; set; }
        public int SenderID { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
        public string MessageBody { get; set; }

        // Default constructor
        public GeneralMessage(int rcvid, int prevmsgid, int sendid, DateTime date, string msg)
        {
            ReceiverID = rcvid;
            MessageID = -1; // ID is created in DB; this is updated after the query is ran -- bad method? Should I create a mapping like with SysID?
            PrevMessageID = prevmsgid;
            SenderID = sendid;
            SentDate = date;
            IsRead = false; // Is it necessary to set IsRead = false if already set to default 0 in db?
            MessageBody = msg;
        }

        // Constructor to create a new message with no previous message; default unread
        public GeneralMessage(int rcvid, int sendid, DateTime date, string msg) 
            : this(rcvid, -1, sendid, date, msg) { }

    }
}
