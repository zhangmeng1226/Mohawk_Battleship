using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class NetDisconnectEvent : NetEvent
    {
        public NetDisconnectEvent()
        {
        }

        public NetDisconnectEvent(Exception e)
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