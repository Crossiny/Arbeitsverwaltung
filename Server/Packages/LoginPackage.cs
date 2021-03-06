// Arbeitsverwaltung/Server/LoginPackage.cs
// by Christoph Schimpf, Jonathan Boeckel
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

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
            info.AddValue("Password", Password);
        }

        #endregion
    }
}