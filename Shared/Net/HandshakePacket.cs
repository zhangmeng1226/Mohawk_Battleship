using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MBC.Shared.Net
{
    [Packet(144)]
    public class HandshakePacket : Packet
    {
        public override void HandleStream(NetworkStream stream)
        {
        }
    }
}