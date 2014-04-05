using MBC.Shared;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Shot"/> that was made by a <see cref="Register"/>.
    /// </summary>
    [Serializable]
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
        /// Gets the <see cref="Shot"/> made by the <see cref="PlayerEvent.Register"/>.
        /// </summary>
        public Shot Shot
        {
            get;
            private set;
        }

        protected internal override void PerformOperation()
        {
            if (!Player.Active)
            {
                throw new InvalidEventException(this, "The player is inactive.");
            }
            if (Shot.Coordinates < new Coordinates(0, 0) || Shot.Coordinates > Player.Match.FieldSize)
            {
                throw new InvalidEventException(this, "Invalid shot made.");
            }
            var shipHit = ShipList.GetShipAt(Shot);
            Player.ShotsMade.Add(Shot);
            if (shipHit != null)
            {
                shipHit.Hit(Shot.Coordinates);
            }
        }
    }
}