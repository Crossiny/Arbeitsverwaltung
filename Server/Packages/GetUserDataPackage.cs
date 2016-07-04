// Arbeitsverwaltung/Server/GetUserDataPackage.cs
// by Christoph Schimpf, Jonathan Boeckel
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packages
{
    [Serializable]
    public class GetUserDataPackage : ISerializable
    {
        public string Username;

        public GetUserDataPackage() { }

        GetUserDataPackage(SerializationInfo info, StreamingContext context)
        {
            Username = info.GetString("Username");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
        }
    }
}
