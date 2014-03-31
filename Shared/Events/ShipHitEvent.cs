using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class ShipHitEvent : ShipEvent
    {
        public ShipHitEvent(Ship ship)
            : base(ship)
        {
        }

        protected internal override void PerformOperation()
        {
        }
    }
}