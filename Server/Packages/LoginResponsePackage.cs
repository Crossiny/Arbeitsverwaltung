// Arbeitsverwaltung/Server/LoginResponsePackage.cs
// by Christoph Schimpf, Jonathan Boeckel
using System;
using System.Runtime.Serialization;

namespace Server.Packages
{
    [Serializable]
    public class LoginResponsePackage : ISerializable
    {
        public bool IsAdmin;
        public bool Success;
        public string Username;

        public LoginResponsePackage()
        {
        }

        private LoginResponsePackage(SerializationInfo info, StreamingContext context)
        {
            Username = info.GetString("Username");
            Success = info.GetBoolean("Success");
            IsAdmin = info.GetBoolean("IsAdmin");
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
            info.AddValue("Success", Success);
            info.AddValue("IsAdmin", IsAdmin);
        }

        #endregion
    }
}