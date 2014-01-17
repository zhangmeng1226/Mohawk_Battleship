using MBC.Core.Rounds;
using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had lost a <see cref="GameLogic"/>.
    /// </summary>
    public class PlayerLostEvent : PlayerEvent
    {
        /// <summary>
        /// Deprecated. Constructs the event with the player ID that lost.
        /// </summary>
        /// <param name="loser"></param>
        [Obsolete("Old framework")]
        public PlayerLostEvent(IDNumber loser)
            : base(loser)
        {
        }

        /// <summary>
        /// Constructs the event with the player that lost.
        /// </summary>
        /// <param name="loser"></param>
        public PlayerLostEvent(Player loser)
            : base(loser)
        {
        }

        /// <summary>
        /// Constructs the event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PlayerLostEvent(SerializationInfo info, StreamingContext context)
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