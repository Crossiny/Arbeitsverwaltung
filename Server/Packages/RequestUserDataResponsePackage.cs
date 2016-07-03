using System;
using System.Runtime.Serialization;
using Server.Database;

namespace Server.Packages
{
    [Serializable]
    internal class RequestUserDataResponsePackage : ISerializable
    {
        public bool Success;
        public User UserData;

        public RequestUserDataResponsePackage()
        {
        }

        private RequestUserDataResponsePackage(SerializationInfo info, StreamingContext context)
        {
            UserData = info.GetValue("User", typeof(User)) as User;
            Success = info.GetBoolean("Success");
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("User", UserData);
            info.AddValue("Success", Success);
        }

        #endregion
    }
}