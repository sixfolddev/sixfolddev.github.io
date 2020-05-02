/*
 * This class represents a general message that is sent and received between users. Implements IMessage.
 */
using System;

namespace RoomAid.ServiceLayer
{
    public class GeneralMessage : IMessage
    {
        private const bool _general = true; // Is a general message
        public int ReceiverID { get; set; }
        public int MessageID { get; set; }
        public int PrevMessageID { get; set; }
        public int SenderID { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentDate { get; set; }
        // Content of a message
        public string MessageBody { get; set; }
        public bool IsGeneral { get; }

        // Default empty constructor
        public GeneralMessage() { }

        // Constructor to create a message reply to a previous message; default unread
        public GeneralMessage(int rcvid, int prevmsgid, int sendid, DateTime date, string msg)
        {
            ReceiverID = rcvid;
            MessageID = -1; // ID is created in DB; this is updated after the query is ran -- bad method? Should I create a mapping like with SysID?
            PrevMessageID = prevmsgid;
            SenderID = sendid;
            SentDate = date;
            IsRead = false; // Is it necessary to set IsRead = false if already set to default 0 in db?
            MessageBody = msg;
            IsGeneral = _general;
        }

        // Constructor to create a new message with no previous message; default unread
        public GeneralMessage(int rcvid, int sendid, DateTime date, string msg) 
            : this(rcvid, -1, sendid, date, msg) { }

        // Overrriden ToString() method that returns the string represntation of a general message
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6}", ReceiverID,
                MessageID, PrevMessageID, SenderID, SentDate, IsRead, MessageBody);
        }
    }
}
