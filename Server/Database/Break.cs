using System;
using System.Runtime.Serialization;

namespace Server.Database
{
    [Serializable]
    public class Break : ISerializable
    {
        public DateTime StartTime, EndTime;

        private Break(SerializationInfo info, StreamingContext context)
        {
            StartTime = info.GetDateTime("StartTime");
            EndTime = info.GetDateTime("EndTime");
        }

        public TimeSpan BreakSpan
        {
            get { return EndTime - StartTime; }
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("StartTime", StartTime);
            info.AddValue("EndTime", EndTime);
        }

        #endregion
    }
}