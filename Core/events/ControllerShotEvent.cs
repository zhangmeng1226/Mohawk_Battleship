using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerShotEvent : ControllerEvent
    {
        private Shot shot;

        public ControllerShotEvent(ControllerRegister register, Shot shot)
            : base(register)
        {
            this.shot = shot;
            StringBuilder msg = new StringBuilder();
            msg.Append(register);
            if (shot != null)
            {
                msg.Append(" shot ");
                if (shot.Receiver < register.Match.Players.Count && shot.Receiver >= 0)
                {
                    msg.Append(register.Match.Players[shot.Receiver]);
                }
                else
                {
                    msg.Append("nobody");
                }
                msg.Append(" at ");
                msg.Append(shot.Coordinates);
            }
            else
            {
                msg.Append(" did not make a shot.");
            }
            message = msg.ToString();
        }

        public Shot Shot
        {
            get
            {
                return shot;
            }
        }
    }
}
