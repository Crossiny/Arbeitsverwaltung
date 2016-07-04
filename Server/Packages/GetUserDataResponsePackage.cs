// Arbeitsverwaltung/Server/GetUserDataResponsePackage.cs
// by Christoph Schimpf, Jonathan Boeckel
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Server.Database;

namespace Server.Packages
{
    [Serializable]
    public class GetUserDataResponsePackage : ISerializable
    {
        public User @User;

        public GetUserDataResponsePackage() { }

        GetUserDataResponsePackage(SerializationInfo info, StreamingContext context)
        {
            User = info.GetValue("User", typeof(User)) as User;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("User", User);
        }
    }
}
