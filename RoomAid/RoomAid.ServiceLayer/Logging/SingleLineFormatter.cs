namespace RoomAid.ServiceLayer
{
    public class SingleLineFormatter : ILogFormatter
    {
        public string FormatLog(LogMessage logMessage)
        {
            return string.Format("{0:yyyy.MM.dd HH:mm:ss},{1},[{2}],{3},{4},{5},{6}", logMessage.Time, 
                logMessage.CallingClass, logMessage.CallingMethod, logMessage.Level, logMessage.UserID, logMessage.SessionID, logMessage.Text);
        }
    }
}
