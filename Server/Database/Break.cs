using System;

namespace Server.Database
{
    class Break
    {
        public DateTime StartTime, EndTime;

        public TimeSpan BreakSpan
        {
            get { return EndTime - StartTime; }
        }
    }
}