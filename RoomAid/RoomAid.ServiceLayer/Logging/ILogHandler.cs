namespace RoomAid.ServiceLayer
{
    public interface ILogHandler
    {
        bool WriteLog(LogMessage logMessage);
        bool DeleteLog(LogMessage logMessage);
    }
}
