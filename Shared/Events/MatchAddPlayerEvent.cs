using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Event created when a player has been added to a match.
    /// </summary>
    public class MatchAddPlayerEvent : MatchEvent
    {
        /// <summary>
        /// Constructs this event with the given player.
        /// </summary>
        /// <param name="player"></param>
        public MatchAddPlayerEvent(Match match, Player player)
            : base(match)
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

        public override bool ApplyBackward()
        {
            return Match.Players.Remove(Player);
        }

        public override bool ApplyForward()
        {
            return Match.Players.Add(Player);
        }
    }
}