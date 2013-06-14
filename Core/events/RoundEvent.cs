using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    /// <summary>
    /// A delegate defining a method to invoke with a RoundEvent.
    /// </summary>
    /// <param name="ev">The Event.</param>
    public delegate void MBCRoundEventHandler(RoundEvent ev);

    public abstract class RoundEvent : Event
    {
        private Round round;

        public RoundEvent(Round rnd)
        {
            round = rnd;
        }

        public Round Round
        {
            get
            {
                return round;
            }
        }
    }
}
