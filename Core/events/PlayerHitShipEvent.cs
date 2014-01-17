using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Created during a match when a player hits an opposing ship.
    /// </summary>
    public class PlayerHitShipEvent : PlayerEvent
    {
        /// <summary>
        /// Deprecated. Constructs the event with a player ID and the shot they made
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="shot"></param>
        [Obsolete("Old framework")]
        public PlayerHitShipEvent(IDNumber sender, Shot shot)
            : base(sender)
        {
            HitShot = shot;
        }

        /// <summary>
        /// Constructs the event with a player object and the shot they made.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="shot"></param>
        public PlayerHitShipEvent(Player sender, Shot shot)
            : base(sender)
        {
            HitShot = shot;
        }

        /// <summary>
        /// Constructs the event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PlayerHitShipEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the shot the player made.
        /// </summary>
        public Shot HitShot
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