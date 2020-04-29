using System;

namespace RoomAid.ServiceLayer.Messaging
{
    class Invitation : IMessage
    {
        public int ReceiverID { get; set; }
        public int MessageID { get; set; }
        public int PrevMessageID { get; set; }
        public int SenderID { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsAccepted { get; set; }

        // Default constructor
        public Invitation(int rcvid, int sendid, DateTime date)
        {
            ReceiverID = rcvid;
            MessageID = -1; // ID is created in DB; this is updated after the query is ran
            PrevMessageID = -1;
            SenderID = sendid;
            SentDate = date;
            IsRead = false; // Is it necessary to set IsRead = false if already set to default 0 in db?
            IsAccepted = false;
        }
    }
}
