using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class ShipHitEvent : ShipEvent
    {
        public ShipHitEvent(Ship ship, Coordinates coords)
            : base(ship)
        {
            HitCoords = coords;
        }

        public Coordinates HitCoords
        {
            get;
            private set;
        }

        protected internal override void PerformOperation()
        {
            if (!Ship.IsAt(HitCoords))
            {
                throw new InvalidEventException(this, String.Format("The specified shot {0} is out of range of ship {1}.", HitCoords, Ship));
            }
            if (!Ship.RemainingLocations.Contains(HitCoords))
            {
                throw new InvalidEventException(this, String.Format("The ship {0} has already been hit at {1}", Ship, HitCoords));
            }
            Ship.RemainingLocations.Remove(HitCoords);
            if (Ship.IsSunk())
            {
                Ship.Sink();
            }
        }
    }
}