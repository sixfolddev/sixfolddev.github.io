using System;

namespace RoomAid.ServiceLayer
{
    public class AnalyzedError
    {
        public Exception E { get; }
        public LogLevels.Levels Lev { get; set; }
        public String Message { get; set; }
        public AnalyzedError(Exception e)
        {
            E = e;
        }
        

    }
}