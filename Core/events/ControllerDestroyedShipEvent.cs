using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerDestroyedShipEvent : RoundControllerEvent
    {
        private Ship destroyed;
        private ControllerUser owner;

        public ControllerDestroyedShipEvent(ControllerUser controller, Round round, Ship destroyedShip, ControllerUser opponent)
            : base(controller, round)
        {
            this.destroyed = destroyedShip;
            this.owner = opponent;

            message = controller + " destroyed a ship at " + destroyedShip + " from " + opponent + ".";
        }
    }
}
