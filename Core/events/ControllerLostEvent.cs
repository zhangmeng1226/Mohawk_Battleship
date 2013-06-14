using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerLostEvent : ControllerEvent
    {
        public ControllerLostEvent(ControllerRegister controller)
            : base(controller)
        {
            message = controller + " has lost the round.";
        }
    }
}
