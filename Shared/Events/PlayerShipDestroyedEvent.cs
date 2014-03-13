using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/>'s <see cref="Ship"/> that has
    /// been destroyed by another <see cref="Register"/>.
    /// </summary>
    [Serializable]
    public class PlayerShipDestroyedEvent : PlayerEvent
    {
        /// <summary>
        /// Constructs this event with the player who owns the destroyed ship.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="destroyedShip"></param>
        public PlayerShipDestroyedEvent(Player owner, Ship destroyedShip) :
            base(owner)
        {
            DestroyedShip = destroyedShip;
        }

        /// <summary>
        /// Gets the <see cref="Ship"/> that was destroyed.
        /// </summary>
        public Ship DestroyedShip
        {
            get;
            private set;
        }
    }
}