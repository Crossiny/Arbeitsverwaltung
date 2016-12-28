// Arbeitsverwaltung/Server/LogoutPackage.cs
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
    class LogoutPackage : ISerializable
    {

        public LogoutPackage() { }

        LogoutPackage(SerializationInfo info, StreamingContext context)
        {
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
