using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public abstract class ShipEvent : Event
    {
        protected ShipEvent(Ship ship)
        {
            Ship = ship;
        }

        public Ship Ship
        {
            get;
            protected set;
        }
    }
}