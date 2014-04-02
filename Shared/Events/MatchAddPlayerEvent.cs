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
            if (Match.Players.Contains(Player))
            {
                throw new InvalidEventException(this, "Match already contains the player.");
            }
            Match.Players.Add(Player);
            Player.Ships = new HashSet<Ship>();
            Player.Match = Match;
            foreach (Ship startShip in Match.StartingShips)
            {
                Ship newShip = new Ship(startShip);
                newShip.Owner = Player;
                Player.Ships.Add(newShip);
            }
        }
    }
}