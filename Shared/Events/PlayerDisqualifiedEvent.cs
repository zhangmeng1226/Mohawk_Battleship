using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had lost a <see cref="GameLogic"/>.
    /// </summary>
    public class PlayerDisqualifiedEvent : PlayerEvent
    {
        /// <summary>
        /// Constructs the event with the player that lost.
        /// </summary>
        /// <param name="loser"></param>
        public PlayerDisqualifiedEvent(Player loser, string reason)
            : base(loser)
        {
            Reason = reason;
        }

        /// <summary>
        /// Constructs the event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PlayerDisqualifiedEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the reason for why this player had been disqualified within a round.
        /// </summary>
        public string Reason
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