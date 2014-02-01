using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had won a <see cref="GameLogic"/>.
    /// </summary>
    public class PlayerWonEvent : PlayerEvent
    {
        /// <summary>
        /// Do not use.
        /// </summary>
        /// <param name="register">The <see cref="Register"/> winning a <see cref="GameLogic"/>.</param>
        [Obsolete("Old framework")]
        public PlayerWonEvent(IDNumber player)
            : base(player)
        {
        }

        /// <summary>
        /// Constructs the event with the player that won.
        /// </summary>
        /// <param name="player"></param>
        public PlayerWonEvent(Player player)
            : base(player)
        {
        }

        /// <summary>
        /// Constructs the event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PlayerWonEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
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