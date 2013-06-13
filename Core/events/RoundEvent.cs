using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
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
