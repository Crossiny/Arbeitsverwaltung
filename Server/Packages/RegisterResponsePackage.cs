// Arbeitsverwaltung/Server/RegisterResponsePackage.cs
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
    public class RegisterResponsePackage : ISerializable
    {
        public bool Success;

        public RegisterResponsePackage() { }

        RegisterResponsePackage(SerializationInfo info, StreamingContext context)
        {
            Success = info.GetBoolean("Success");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Success", Success);
        }
    }
}
