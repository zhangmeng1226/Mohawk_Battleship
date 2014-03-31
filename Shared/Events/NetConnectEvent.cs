using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class NetConnectEvent : NetEvent
    {
        public NetConnectEvent(long syncTime)
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