using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer.Messaging
{
    class Invitation : IMessage
    {
        public string SysID { get; set; }
        public string MessageID { get; set; }
        public string PrevMessageID { get; set; }
        public string SenderID { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
        public enum Response
        {
            
        }
    }
}
