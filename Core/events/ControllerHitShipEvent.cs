using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerHitShipEvent : RoundControllerEvent
    {
        private Coordinates coords;
        private ControllerUser opponent;

        public ControllerHitShipEvent(ControllerUser controller, Round round, Coordinates coords, ControllerUser opposer)
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

        public ControllerUser Opponent
        {
            get
            {
                return opponent;
            }
        }
    }
}
