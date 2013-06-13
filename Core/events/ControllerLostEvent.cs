using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerLostEvent : RoundControllerEvent
    {
        public ControllerLostEvent(Controller controller, Round round)
            : base(controller, round)
        {
            message = controller + " has lost the round.";
        }
    }
}
