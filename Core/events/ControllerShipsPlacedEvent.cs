using MBC.Core.Rounds;
using MBC.Shared;
using System;
using System.Text;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="ControllerRegister"/>'s <see cref="ShipList"/> that had
    /// been requested to be modified.
    /// </summary>
    public class ControllerShipsPlacedEvent : ControllerEvent
    {
        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the rest of the parameters,
        /// and generates a message based on the state of the given <see cref="ShipList"/>.
        /// </summary>
        /// <param name="register">A <see cref="ControllerRegister"/>.</param>
        /// <param name="newShips">The <see cref="ShipList"/> associated with the <see cref="ControllerRegister"/></param>
        public ControllerShipsPlacedEvent(ControllerID register, ShipList oldShips, ShipList newShips)
            : base(register)
        {
            Ships = newShips;
            PrevShips = oldShips;
        }

        protected internal override void GenerateMessage()
        {

            StringBuilder msg = new StringBuilder();
            msg.Append(RegisterID);
            int placedCount = 0;
            foreach (var ship in Ships.Ships)
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
            if (placedCount > 0 && !Ships.ShipsPlaced)
            {
                msg.Append(" and ");
            }
            if (!Ships.ShipsPlaced)
            {
                placedCount = 0;
                msg.Append(" did not place ");
                foreach (var ship in Ships.Ships)
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
            Message = msg.ToString();
        }

        public ShipList PrevShips
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="ShipList"/> of the <see cref="Controller.Register"/>.
        /// </summary>
        public ShipList Ships
        {
            get;
            private set;
        }

        internal override void ProcBackward(Round round)
        {
            round.Registers[RegisterID].Ships = PrevShips;
            round.Registers[RegisterID].ShipsLeft = new ShipList(PrevShips.Ships);
        }

        internal override void ProcForward(Round round)
        {
            round.Registers[RegisterID].Ships = Ships;
            round.Registers[RegisterID].ShipsLeft = new ShipList(Ships.Ships);
        }
    }
}