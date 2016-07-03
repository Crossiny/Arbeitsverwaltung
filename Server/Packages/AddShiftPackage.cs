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
    class AddShiftPackage : ISerializable
    {
        public Shift shift;

        AddShiftPackage(SerializationInfo info, StreamingContext context)
        {
            shift = info.GetValue("Shift", typeof(Shift)) as Shift;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Shift", shift);
        }
    }
}
