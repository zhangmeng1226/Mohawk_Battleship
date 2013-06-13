using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public abstract class RoundControllerEvent : RoundEvent
    {
        private Controller controller;

        public RoundControllerEvent(Controller controller, Round round)
            : base(round)
        {
            this.controller = controller;
        }

        public Controller Controller
        {
            get
            {
                return controller;
            }
        }
    }
}
