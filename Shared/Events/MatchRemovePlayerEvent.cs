using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Created during a match when a player is removed.
    /// </summary>
    public class MatchRemovePlayerEvent : Event
    {
        /// <summary>
        /// Constructs this event with the player being removed.
        /// </summary>
        /// <param name="player"></param>
        public MatchRemovePlayerEvent(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Constructs this event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public MatchRemovePlayerEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the player that has been removed.
        /// </summary>
        public Player Player
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the serialization data from the event
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}