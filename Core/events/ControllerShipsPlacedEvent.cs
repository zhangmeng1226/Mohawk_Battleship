using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerShipsPlacedEvent : RoundControllerEvent
    {
        private ShipList shipsPlaced;

        public ControllerShipsPlacedEvent(ControllerUser controller, Round round, ShipList ships)
            : base(controller, round)
        {
            shipsPlaced = ships;

            StringBuilder msg = new StringBuilder();
            msg.Append(controller);
            int placedCount = 0;
            foreach (var ship in shipsPlaced.Ships)
            {
                if (ship.IsPlaced)
                {
                    if (placedCount++ == 0)
                    {
                        msg.Append(" placed ");
                    }
                    else
                    {
                        msg.Append(", ");
                    }
                    msg.Append(ship);
                }
            }
            if (placedCount > 0 && !shipsPlaced.ShipsPlaced)
            {
                msg.Append(" and ");
            }
            if (!shipsPlaced.ShipsPlaced)
            {
                placedCount = 0;
                msg.Append(" did not place ");
                foreach (var ship in shipsPlaced.Ships)
                {
                    if (!ship.IsPlaced)
                    {
                        if (placedCount++ != 0)
                        {
                            msg.Append(", ");
                        }
                        msg.Append(ship);
                    }
                }
            }
            msg.Append(".");
            message = msg.ToString();
        }
    }
}
