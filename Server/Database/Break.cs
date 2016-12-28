// Arbeitsverwaltung/Server/Break.cs
// by Christoph Schimpf, Jonathan Boeckel
using System;
using System.Runtime.Serialization;

namespace Server.Database
{
    [Serializable]
    public class Break : ISerializable
    {
        public DateTime StartTime, EndTime;

        public Break() { }

        private Break(SerializationInfo info, StreamingContext context)
        {
            StartTime = info.GetDateTime("StartTime");
            EndTime = info.GetDateTime("EndTime");
        }

        public TimeSpan BreakSpan
        {
            get { return EndTime - StartTime; }
        }
        
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("StartTime", StartTime);
            info.AddValue("EndTime", EndTime);
        }
    }
}