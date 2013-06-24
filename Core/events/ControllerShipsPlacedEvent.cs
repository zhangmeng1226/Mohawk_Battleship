using MBC.Shared;
using System.Text;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="ControllerRegister"/>'s <see cref="ShipList"/> that had
    /// been requested to be modified.
    /// </summary>
    public class ControllerShipsPlacedEvent : ControllerEvent
    {
        private ShipList shipsPlaced;

        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the rest of the parameters,
        /// and generates a message based on the state of the given <see cref="ShipList"/>.
        /// </summary>
        /// <param name="register">A <see cref="ControllerRegister"/>.</param>
        /// <param name="ships">The <see cref="ShipList"/> associated with the <see cref="ControllerRegister"/></param>
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

        /// <summary>
        /// Gets the <see cref="ShipList"/> of the <see cref="Controller.Register"/>.
        /// </summary>
        public ShipList Ships
        {
            get
            {
                return shipsPlaced;
            }
        }
    }
}