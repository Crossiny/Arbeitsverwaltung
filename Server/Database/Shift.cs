using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Server.Database
{
    [Serializable]
    public class Shift : ISerializable
    {
        public List<Break> Breaks = new List<Break>();
        public DateTime StartTime, EndTime;

        private Shift(SerializationInfo info, StreamingContext context)
        {
            StartTime = info.GetDateTime("StartTime");
            EndTime = info.GetDateTime("EndTime");
            Breaks = info.GetValue("Breaks", typeof(List<Break>)) as List<Break>;
        }

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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("StartTime", StartTime);
            info.AddValue("EndTime", EndTime);
            info.AddValue("Breaks", Breaks);
        }
    }
}