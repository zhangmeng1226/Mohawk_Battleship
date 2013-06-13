using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerHitShipEvent : RoundControllerEvent
    {
        private Coordinates coords;
        private Controller opponent;

        public ControllerHitShipEvent(Controller controller, Round round, Coordinates coords, Controller opposer)
            : base(controller, round)
        {
            this.coords = coords;
            this.opponent = opposer;

            message = controller + " hit a " + opposer + " ship at " + coords;
        }

        public Coordinates HitCoords
        {
            get
            {
                return coords;
            }
        }

        public Controller Opponent
        {
            get
            {
                return opponent;
            }
        }
    }
}
