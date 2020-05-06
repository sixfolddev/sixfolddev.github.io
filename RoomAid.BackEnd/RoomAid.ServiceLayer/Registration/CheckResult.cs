

namespace RoomAid.ServiceLayer
{
    public class CheckResult : IResult
    {

        public CheckResult(string reason, bool IsSuccess)
        {
            this.Message = reason;
            this.IsSuccess = IsSuccess;
        }

        public string Message { get; }

        public bool IsSuccess { get; }
    }
}
