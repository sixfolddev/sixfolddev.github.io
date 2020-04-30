using System;

namespace RoomAid.ServiceLayer.Messaging
{
    interface IQueueHandler
    {
        bool Send(string message);
        bool Receive(string message);
        void Create(string path);
        void Delete(string path);
    }
}
