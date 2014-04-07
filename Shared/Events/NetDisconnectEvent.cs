using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class NetDisconnectEvent : NetEvent
    {
        public NetDisconnectEvent(NetCom communicator)
            : base(communicator)
        {
        }

        public NetDisconnectEvent(NetCom communicator, Exception e)
            : this(communicator)
        {
            Exception = e;
        }

        public Exception Exception
        {
            get;
            private set;
        }
    }
}