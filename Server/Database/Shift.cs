using System;
using System.Collections.Generic;

namespace Server.Database
{
    class Shift
    {
        public DateTime StartTime, EndTime;

        public TimeSpan WorkSpan
        {
            get
            {
                TimeSpan span = EndTime - StartTime;
                foreach (Break tBreak in Breaks)
                {
                    span -= tBreak.BreakSpan;
                }
                return span;
            }
        }

        public TimeSpan BreakSpan
        {
            get
            {
                TimeSpan breakSpan = default(TimeSpan);
                foreach (Break tBreak in Breaks)
                {
                    breakSpan += tBreak.BreakSpan;
                }
                return breakSpan;
            }
        }
        public List<Break> Breaks = new List<Break>();
    }
}