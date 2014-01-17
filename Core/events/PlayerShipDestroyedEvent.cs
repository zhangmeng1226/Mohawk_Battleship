using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/>'s <see cref="Ship"/> that has
    /// been destroyed by another <see cref="Register"/>.
    /// </summary>
    public class PlayerShipDestroyedEvent : PlayerEvent
    {
        /// <summary>
        /// Deprecated. Constructs the event with the player ID who owns the destroyed ship.
        /// </summary>
        /// <param name="register">The <see cref="Register"/> that destroyed the <see cref="Ship"/>.</param>
        /// <param name="shipOwner">The <see cref="Register"/> that owns the destroyed <see cref="Ship"/></param>
        /// <param name="destroyedShip">The destroyed <see cref="Ship"/>.</param>
        [Obsolete("Old framework")]
        public PlayerShipDestroyedEvent(IDNumber owner, Ship destroyedShip)
            : base(owner)
        {
            DestroyedShip = destroyedShip;
        }

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
        /// Constructs the event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PlayerShipDestroyedEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the <see cref="Ship"/> that was destroyed.
        /// </summary>
        public Ship DestroyedShip
        {
            get;
            private set;
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