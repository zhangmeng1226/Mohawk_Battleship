using System.Runtime.Serialization;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/>'s <see cref="Ship"/> that has
    /// been destroyed by another <see cref="Register"/>.
    /// </summary>
    public class PlayerShipDestroyedEvent : PlayerEvent
    {
        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor, stores the other parameters,
        /// and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">The <see cref="Register"/> that destroyed the <see cref="Ship"/>.</param>
        /// <param name="shipOwner">The <see cref="Register"/> that owns the destroyed <see cref="Ship"/></param>
        /// <param name="destroyedShip">The destroyed <see cref="Ship"/>.</param>
        public PlayerShipDestroyedEvent(IDNumber owner, Ship destroyedShip)
            : base(owner)
        {
            DestroyedShip = destroyedShip;
        }

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

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}