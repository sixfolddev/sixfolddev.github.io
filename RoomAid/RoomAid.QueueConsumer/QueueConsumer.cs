using RoomAid.ManagerLayer;
using RoomAid.ServiceLayer;
using System;
using System.Threading;

namespace RoomAid.QueueConsumer
{
    class QueueConsumer
    {
        static void Main(string[] args)
        {
            IQueueHandler queue = new MSMQHandler();
            double timeout = 1;
            int sleepTime = 10000;

            while(true)
            {
                if(!queue.Peek(timeout))
                {
                    Thread.Sleep(sleepTime); // Sleep for 10 seconds
                    continue;
                }
                IMessage message = (IMessage)queue.Receive();
            }
        }
    }
}
