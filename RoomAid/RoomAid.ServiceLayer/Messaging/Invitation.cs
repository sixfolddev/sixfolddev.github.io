/*
 * This class represents an invitation that is sent and received between users. Implements IMessage.
 */
using System;

namespace RoomAid.ServiceLayer
{
    public class Invitation : IMessage
    {
        private const bool _general = false; // Is not a general message
        public int ReceiverID { get; set; }
        public int MessageID { get; set; }
        public int PrevMessageID { get; set; }
        public int SenderID { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentDate { get; set; }
        // Indicates whether an invitation is accepted by a user or not
        public bool IsAccepted { get; set; }
        public bool IsGeneral { get; }
        // Default empty constructor
        public Invitation() { }

        // Constructor to create an invitation reply to an invitation; default unread and not accepted
        public Invitation(int rcvid, int prevmsgid, int sendid, DateTime date)
        {
            ReceiverID = rcvid;
            MessageID = -1; // ID is created in DB; this is updated after the query is ran
            PrevMessageID = prevmsgid;
            SenderID = sendid;
            SentDate = date;
            IsRead = false; // Is it necessary to set IsRead = false if already set to default 0 in db?
            IsAccepted = false;
            IsGeneral = _general;
        }

        // Constructor to create a new invitation; default unread and not accepted
        public Invitation(int rcvid, int sendid, DateTime date)
            : this(rcvid, -1, sendid, date) { }

        // Overrriden ToString() method that returns the string represntation of an invitation
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6}", ReceiverID,
                MessageID, PrevMessageID, SenderID, SentDate, IsRead, IsAccepted);

        }
    }
}
