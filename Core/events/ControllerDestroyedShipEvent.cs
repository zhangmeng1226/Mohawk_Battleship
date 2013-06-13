using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerDestroyedShipEvent : RoundControllerEvent
    {
        private Ship destroyed;
        private Controller owner;

        public ControllerDestroyedShipEvent(Controller controller, Round round, Ship destroyedShip, Controller opponent)
            : base(controller, round)
        {
            this.destroyed = destroyedShip;
            this.owner = opponent;

            message = controller + " destroyed a ship at " + destroyedShip + " from " + opponent + ".";
        }
    }
}
