using System;
using System.Runtime.Serialization;

namespace Server.Packages
{
    [Serializable]
    public class LoginPackage : ISerializable
    {
        public string Password;
        public string Username;

        public LoginPackage()
        {
        }

        private LoginPackage(SerializationInfo info, StreamingContext context)
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