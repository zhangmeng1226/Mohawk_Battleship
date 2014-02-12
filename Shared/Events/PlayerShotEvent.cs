using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Shot"/> that was made by a <see cref="Register"/>.
    /// </summary>
    public class PlayerShotEvent : PlayerEvent
    {
        /// <summary>
        /// Constructs the event with the player who made the shot. No ship hit.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="shot"></param>
        public PlayerShotEvent(Player player, Shot shot)
            : base(player)
        {
            Shot = shot;
        }

        /// <summary>
        /// Constructs the event with the player who made the shot and hit a ship.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="shot"></param>
        /// <param name="shipHit"></param>
        public PlayerShotEvent(Player player, Shot shot, Ship shipHit)
            : base(player)
        {
            Shot = shot;
            ShipHit = shipHit;
        }

        /// <summary>
        /// Gets a value indicating if the shot hit a ship.
        /// </summary>
        public bool Hit
        {
            get
            {
                return ShipHit != null;
            }
        }

        /// <summary>
        /// Gets a ship object that the shot hit, if it hit. Otherwise the ship will be null.
        /// </summary>
        public Ship ShipHit
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="Shot"/> made by the <see cref="PlayerEvent.Register"/>.
        /// </summary>
        public Shot Shot
        {
            get;
            private set;
        }
    }
}