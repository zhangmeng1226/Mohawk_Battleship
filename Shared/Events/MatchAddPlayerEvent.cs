using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Event created when a player has been added to a match.
    /// </summary>
    [Serializable]
    public class MatchAddPlayerEvent : Event
    {
        /// <summary>
        /// Constructs this event with the given player.
        /// </summary>
        /// <param name="player"></param>
        internal MatchAddPlayerEvent(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// The player associated with this event.
        /// </summary>
        public Player Player
        {
            get;
            private set;
        }
    }
}