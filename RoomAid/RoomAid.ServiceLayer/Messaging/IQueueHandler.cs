using System;

namespace RoomAid.ServiceLayer.Messaging
{
    interface IQueueHandler
    {
        void Send(string message);
        string Receive();
        void Create(string path);
        void Delete(string path);
    }
}
