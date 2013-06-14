using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerShotEvent : RoundControllerEvent
    {
        private Coordinates coords;
        private ControllerUser opponent;

        public ControllerShotEvent(ControllerUser controller, Round round, Coordinates coords, ControllerUser opposer)
            : base(controller, round)
        {
            this.coords = coords;
            this.opponent = opposer;

            StringBuilder msg = new StringBuilder();
            msg.Append(controller);
            msg.Append(" made a shot against ");
            if (opposer != null)
            {
                msg.Append(opposer);
            }
            else
            {
                msg.Append("nobody");
            }
            msg.Append(" at ");
            msg.Append(coords);
            message = msg.ToString();
        }

        public Coordinates ShotCoordinates
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
