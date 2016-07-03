using System;
using System.Runtime.Serialization;
using Server.Database;

namespace Server.Packages
{
    [Serializable]
    public class AddShiftResponsePackage : ISerializable
    {
        public bool Success;
        public User NewUser;

        public AddShiftResponsePackage()
        {
        }

        private AddShiftResponsePackage(SerializationInfo info, StreamingContext context)
        {
            NewUser = info.GetValue("NewUser", typeof(User)) as User;
            Success = info.GetBoolean("Success");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Success", Success);
            info.AddValue("NewUser", NewUser);
        }
     }
}