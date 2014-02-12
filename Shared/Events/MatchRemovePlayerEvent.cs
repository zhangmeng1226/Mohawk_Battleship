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
        /// Gets the player that has been removed.
        /// </summary>
        public Player Player
        {
            get;
            private set;
        }
    }
}