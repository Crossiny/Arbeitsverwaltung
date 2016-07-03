using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packages
{
    [Serializable]
    class LogoutResponsePackage : ISerializable
    {
        public bool Success;
        public LogoutResponsePackage() { }

        LogoutResponsePackage(SerializationInfo info, StreamingContext context)
        {
            Success = info.GetBoolean("Success");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Success", Success);
        }
    }
}
