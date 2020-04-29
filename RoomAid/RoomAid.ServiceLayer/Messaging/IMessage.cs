using System;

namespace RoomAid.ServiceLayer.Messaging
{
    interface IMessage
    {
        // ID of the user receiving the message
        int ReceiverID { get; set; }
        // ID of the current message
        int MessageID { get; set; }
        // ID of the message that was replied to (if any)
        int PrevMessageID { get; set; }
        // System ID of the user sending the message
        int SenderID { get; set; }
        // Timestamp of when a message is sent
        DateTime SentDate { get; set; }
        // Indicates whether a message is read or unread
        bool IsRead { get; set; }
        
    }
}
