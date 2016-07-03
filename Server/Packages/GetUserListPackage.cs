
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packages
{
    [Serializable]
    public class GetUserListPackage : ISerializable
    {
        public string Username;
        public GetUserListPackage() { }

        GetUserListPackage(SerializationInfo info, StreamingContext context)
        {
            Username = info.GetString("Username");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username",Username);
        }
    }
}
