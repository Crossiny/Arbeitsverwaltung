using System;
using System.Runtime.Serialization;

namespace Server.Packages
{
    [Serializable]
    internal class RequestUserDataPackage : ISerializable
    {
        public bool Success;
        public string Username;

        public RequestUserDataPackage()
        {
        }

        private RequestUserDataPackage(SerializationInfo info, StreamingContext context)
        {
            Username = info.GetString("Username");
            Success = info.GetBoolean("Success");
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
            info.AddValue("Success", Success);
        }

        #endregion
    }
}