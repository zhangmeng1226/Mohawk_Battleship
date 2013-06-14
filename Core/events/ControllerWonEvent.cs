using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerWonEvent : ControllerEvent
    {
        public ControllerWonEvent(ControllerRegister register)
            : base(register)
        {
            message = register + " has won the round.";
        }
    }
}
