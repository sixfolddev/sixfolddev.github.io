using System.Collections;

namespace RoomAid.ServiceLayer
{
    public interface IQueueHandler
    {
        bool Peek(double timeout);
        void Send(IMessage message);
        IMessage Receive();
    }
}
