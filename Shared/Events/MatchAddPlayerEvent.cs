using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Event created when a player has been added to a match.
    /// </summary>
    [Serializable]
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

        protected internal override void PerformOperation()
        {
            Match.Players.Add(Player);
            Player.Ships = new HashSet<Ship>();
            foreach (Ship startShip in Match.StartingShips)
            {
                Player.Ships.Add(new Ship(startShip));
            }
        }
    }
}