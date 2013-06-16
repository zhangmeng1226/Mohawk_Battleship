using MBC.Shared;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerShipsPlacedEvent : ControllerEvent
    {
        private ShipList shipsPlaced;

        public ControllerShipsPlacedEvent(ControllerRegister register, ShipList ships)
            : base(register)
        {
            shipsPlaced = ships;

            StringBuilder msg = new StringBuilder();
            msg.Append(register);
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