using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerShotEvent : ControllerEvent
    {
        private Shot coords;
        private ControllerRegister opponent;

        public ControllerShotEvent(ControllerRegister register, ControllerRegister opposer, Shot shot)
            : base(register)
        {
            this.coords = shot;
            this.opponent = opposer;

            StringBuilder msg = new StringBuilder();
            msg.Append(register);
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
            msg.Append(shot.Coordinates);
            message = msg.ToString();
        }

        public Shot Shot
        {
            get
            {
                return coords;
            }
        }

        public ControllerRegister Opponent
        {
            get
            {
                return opponent;
            }
        }
    }
}
