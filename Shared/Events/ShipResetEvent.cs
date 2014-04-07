using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class ShipResetEvent : ShipEvent
    {
        public ShipResetEvent(Ship ship)
            : base(ship)
        {
        }

        protected internal override void PerformOperation()
        {
            if (!Ship.IsPlaced)
            {
                throw new InvalidEventException(this, String.Format("Ship {0} is already reset.", Ship));
            }
            Ship.RemainingLocations.Clear();
            Ship.Locations.Clear();
            Ship.Location = default(Coordinates);
            Ship.Orientation = default(ShipOrientation);
            Ship.IsPlaced = false;
            Ship.Active = true;
        }
    }
}