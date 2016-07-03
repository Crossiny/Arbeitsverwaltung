using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packages
{
    [Serializable]
    public class GetUserListResponsePackage : ISerializable
    {
        public List<string> UserList;

        public GetUserListResponsePackage() { }

        GetUserListResponsePackage(SerializationInfo info, StreamingContext context)
        {
            UserList = info.GetValue("UserList", typeof(List<string>)) as List<string>;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UserList", UserList);
        }
    }
}
