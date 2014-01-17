using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/>'s <see cref="ShipList"/> that had
    /// been requested to be modified.
    /// </summary>
    public class PlayerShipsPlacedEvent : PlayerEvent
    {
        private List<Ship> shipList;

        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the rest of the parameters,
        /// and generates a message based on the state of the given <see cref="ShipList"/>.
        /// </summary>
        /// <param name="register">A <see cref="Register"/>.</param>
        /// <param name="newShips">The <see cref="ShipList"/> associated with the <see cref="Register"/></param>
        [Obsolete("Old framework")]
        public PlayerShipsPlacedEvent(IDNumber plr, ShipList newShips)
            : base(plr)
        {
            shipList = new List<Ship>(newShips.Ships);
        }

        /// <summary>
        /// Constructs the event with the player that placed ships.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="newShips"></param>
        public PlayerShipsPlacedEvent(Player player, List<Ship> newShips)
            : base(player)
        {
            shipList = newShips;
        }

        /// <summary>
        /// Constructs the event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PlayerShipsPlacedEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Do not use.
        /// </summary>
        [Obsolete()]
        public ShipList Ships
        {
            get
            {
                return new ShipList(shipList);
            }
        }

        /// <summary>
        /// Gets a list of ships placed.
        /// </summary>
        public List<Ship> ShipsList
        {
            get
            {
                return new List<Ship>(shipList);
            }
        }

        /// <summary>
        /// Gets the serialization data from the event.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}