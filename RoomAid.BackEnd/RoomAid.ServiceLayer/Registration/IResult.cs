using System;

namespace RoomAid.ServiceLayer
{
    public interface IResult
    {
        string Message { get; }
        bool IsSuccess { get; }
    }
}
