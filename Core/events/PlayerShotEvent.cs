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
        /// Passes the <paramref name="register"/> to the base constructor, stores the <paramref name="shot"/>,
        /// and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">A <see cref="Register"/> making the <paramref name="shot"/></param>
        /// <param name="shot">The <see cref="Shot"/> made by the <paramref name="register"/>.</param>
        [Obsolete("Old framework")]
        public PlayerShotEvent(IDNumber shooter, Shot shot)
            : base(shooter)
        {
            Shot = shot;
        }

        /// <summary>
        /// Constructs the event with the player who made the shot.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="shot"></param>
        public PlayerShotEvent(Player player, Shot shot)
            : base(player)
        {
            Shot = shot;
        }

        /// <summary>
        /// Constructs the event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PlayerShotEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the <see cref="Shot"/> made by the <see cref="PlayerEvent.Register"/>.
        /// </summary>
        public Shot Shot
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