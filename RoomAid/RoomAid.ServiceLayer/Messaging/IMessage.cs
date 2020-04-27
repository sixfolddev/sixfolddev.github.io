using System;

namespace RoomAid.ServiceLayer.Messaging
{
    interface IMessage
    {
        // ID of the user receiving the message
        string SysID { get; set; }
        // ID of the current message
        string MessageID { get; set; }
        // ID of the message that was replied to (if any)
        string PrevMessageID { get; set; }
        // System ID of the user sending the message
        string SenderID { get; set; }
        // Timestamp of when a message is sent
        DateTime SentDate { get; set; }
        // Indicates whether a message is read or unread
        bool IsRead { get; set; }
        
    }
}
