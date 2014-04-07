using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class NetIncomingEvent : NetEvent
    {
        public NetIncomingEvent(NetCom communicator, Event incomingEvent)
            : base(communicator)
        {
            ReceivedEvent = incomingEvent;
        }

        public Event ReceivedEvent
        {
            get;
            private set;
        }
    }
}