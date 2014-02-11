using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Event created when a player has been added to a match.
    /// </summary>
    public class MatchAddPlayerEvent : Event
    {
        /// <summary>
        /// Constructs this event with the given player.
        /// </summary>
        /// <param name="player"></param>
        public MatchAddPlayerEvent(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Constructs this event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public MatchAddPlayerEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// The player associated with this event.
        /// </summary>
        public Player Player
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets serialization data from this event.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}