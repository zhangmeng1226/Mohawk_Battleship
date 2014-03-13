using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace MBC.Shared.Net
{
    public abstract class Packet
    {
        private static Dictionary<int, Packet> packetCache;

        public static Packet GetPacket(int packetId)
        {
            var result = packetCache[packetId];
            if (result == null)
            {
                Assembly currentAssembly = Assembly.GetAssembly(typeof(Packet));
                foreach (var type in currentAssembly.GetTypes())
                {
                    var baseType = type.BaseType;
                    while (baseType != null && baseType != typeof(object))
                    {
                        if (baseType == typeof(Packet))
                        {
                            foreach (var attrib in (PacketAttribute[])type.GetCustomAttributes(typeof(PacketAttribute), false))
                            {
                                if (attrib.Identifier == packetId)
                                {
                                    result = (Packet)Activator.CreateInstance(type);
                                    packetCache.Add(attrib.Identifier, result);
                                    return result;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        public abstract void HandleStream(NetworkStream stream);
    }
}