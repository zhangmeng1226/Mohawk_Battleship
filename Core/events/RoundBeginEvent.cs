using MBC.Core.Rounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Round"/> that has begun.
    /// </summary>
    public class RoundBeginEvent : RoundEvent
    {
        /// <summary>
        /// Passes the <paramref name="round"/> to the base constructor and generates a <see cref="Event.Message"/>
        /// based on the <see cref="MBC.Shared.ControllerRegister"/>s that are involved in it.
        /// </summary>
        /// <param name="round">The associated <see cref="Round"/>.</param>
        public RoundBeginEvent(IEnumerable<Player> roundControllers)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("A round has begun with ");
            int controllerCount = 0;
            foreach (var controller in roundControllers)
            {
                if ((controller != roundControllers.Last()) && (controllerCount++ != 0))
                {
                    msg.Append(", ");
                }
                else if (controller == roundControllers.Last())
                {
                    msg.Append(" and ");
                }
                msg.Append(controller);
            }
            msg.Append('.');

            this.Message = msg.ToString();
        }

        protected internal override void GenerateMessage()
        {

        }
    }
}