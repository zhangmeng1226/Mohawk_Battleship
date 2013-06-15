using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class RoundBeginEvent : RoundEvent
    {
        public RoundBeginEvent(Round round) : base(round)
        {
            var roundControllers = round.Registers;

            StringBuilder msg = new StringBuilder();
            msg.Append("A round has begun with ");
            int controllerCount = 0;
            foreach (var controller in roundControllers)
            {
                if ((controller != roundControllers.Last()) && (controllerCount++ != 0))
                {
                    msg.Append(", ");
                }
                else if(controller == roundControllers.Last())
                {
                    msg.Append(" and ");
                }
                msg.Append(controller);
            }
            msg.Append('.');

            this.message = msg.ToString();
        }
    }
}
