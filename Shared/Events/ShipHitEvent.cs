using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class ShipHitEvent : ShipEvent
    {
        public ShipHitEvent(Ship ship, int hitIdx)
            : base(ship)
        {
            HitIndex = hitIdx;
        }

        public int HitIndex
        {
            get;
            private set;
        }

        protected internal override void PerformOperation()
        {
            if (HitIndex > Ship.Length || HitIndex < 1)
            {
                throw new IndexOutOfRangeException(String.Format("The specified index {0} is out of range.", HitIndex));
            }
            Ship.Shots |= 1 << HitIndex;
            if (Ship.IsSunk())
            {
                Ship.Sink();
            }
        }
    }
}