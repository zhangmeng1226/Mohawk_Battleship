using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Net
{
    public class PacketAttribute : Attribute
    {
        public readonly int Identifier;

        public PacketAttribute(int id)
        {
            Identifier = id;
        }
    }
}