using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public abstract class RoundControllerEvent : RoundEvent
    {
        private ControllerUser controller;

        public RoundControllerEvent(ControllerUser controller, Round round)
            : base(round)
        {
            this.controller = controller;
        }

        public ControllerUser Controller
        {
            get
            {
                return controller;
            }
        }
    }
}
