using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerWonEvent : RoundControllerEvent
    {
        public ControllerWonEvent(ControllerUser controller, Round round)
            : base(controller, round)
        {
            message = controller + " has won the round.";
        }
    }
}
