using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packages
{
    [Serializable]
    public class RegisterPackage : ISerializable
    {
        public string Username;
        public string Password;

        public RegisterPackage() { }

        RegisterPackage(SerializationInfo info, StreamingContext context)
        {
            Username = info.GetString("Username");
            Password = info.GetString("Password");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
            info.AddValue("Password", Password);
        }
    }
}
