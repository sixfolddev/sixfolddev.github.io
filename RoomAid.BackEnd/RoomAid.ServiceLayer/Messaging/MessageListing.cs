﻿using System;

namespace RoomAid.ServiceLayer
{
    public class MessageListing
    {
        // ID of the current message
        public int MessageID { get; set; }
        // Indicates whether a message is read or unread
        public bool IsRead { get; set; }
        // Timestamp of when a message is sent
        public DateTime SentDate { get; set; }
        // First and last name of user who sent the message
        public string FullName { get; set; }
        public MessageListing()
        {

        }

        public MessageListing(int msgid, bool read, DateTime date, string name)
        {
            MessageID = msgid;
            IsRead = read;
            SentDate = date;
            FullName = name;
        }
    }
}
