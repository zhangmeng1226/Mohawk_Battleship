using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class NetEvent : Event
    {
        public NetEvent(NetCom communicator)
            : base(communicator)
        {
        }
    }
}