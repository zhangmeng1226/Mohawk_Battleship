using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class NetConnectEvent : NetEvent
    {
        public NetConnectEvent(NetCom communicator, long syncTime)
            : base(communicator)
        {
            SyncTime = syncTime;
        }

        public long SyncTime
        {
            get;
            private set;
        }
    }
}